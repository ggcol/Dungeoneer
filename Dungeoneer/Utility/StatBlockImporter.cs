﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace Dungeoneer.Utility
{
	public static class StatBlockImporter
	{
		public static List<int> GetNumbersFromString(string input)
		{
			string[] tokens = Regex.Split(input, @"\D+");
			List<int> numbers = new List<int>();
			foreach (string token in tokens)
			{
				try
				{
					numbers.Add(Convert.ToInt32(token));
				}
				catch (FormatException)
				{ }
			}

			return numbers;
		}

		public static Model.Creature ParseText(string text)
		{
			Model.CreatureAttributes attributes = new Model.CreatureAttributes();

			if (text != null && text != "")
			{
				string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
				string currentLine = "";
				try
				{
					foreach (string line in lines)
					{
						if (line != "")
						{
							currentLine = line;
							string[] splitLine = line.Split(':');
							string identifier = splitLine[0];
							string entry = splitLine[1];
							List<int> numbers = GetNumbersFromString(entry);
							string[] words = splitLine[1].Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

							if (identifier == "Size/Type")
							{
								attributes.Size = Methods.GetSizeFromString(words[0]);
								string typeStr = string.Join(" ", words.Skip(1));

								string typePattern = @"\s*(?<Type>\w+)\s*(\((?<SubTypes>\D+)\))?";
								Regex typeRegex = new Regex(typePattern, RegexOptions.IgnoreCase);
								Match typeMatch = typeRegex.Match(typeStr);

								if (typeMatch.Success)
								{
									attributes.Type = Methods.GetCreatureTypeFromString(typeMatch.Groups["Type"].Value);
									/*
									if (typeMatch.Groups.Count > 1)
									{
										string subTypes = typeMatch.Groups["SubTypes"].Value;

										string subTypePattern = @"\s*(?<Type>\w+)\s*(\((?<SubTypes>\D+)\))?";
										Regex subTypeRegex = new Regex(subTypePattern, RegexOptions.IgnoreCase);
										Match subTypeMatch = subTypeRegex.Match(entry);

										if (subTypeMatch.Success)
										{
											attributes.Subtypes = Methods.GetCreatureSubTypeFromString();
										}
									}
									*/
								}
							}
							else if (identifier == "Hit Dice")
							{
								string hpPattern = @"\s?(?<NumHD>\d+)(?<HDType>[dD]\d+)\s?(?<HDMod>[\+\-]\d+)?\s?\((?<HP>\d+)\s?hp\)";
								Regex hpRegex = new Regex(hpPattern, RegexOptions.IgnoreCase);
								Match hpMatch = hpRegex.Match(entry);

								if (hpMatch.Success)
								{
									attributes.HitDice = Convert.ToInt32(hpMatch.Groups["NumHD"].Value);
									attributes.HitDieType = Methods.GetDieTypeFromString(hpMatch.Groups["HDType"].Value);
									attributes.HitPoints = Convert.ToInt32(hpMatch.Groups["HP"].Value);
								}
							}
							else if (identifier == "Initiative")
							{
								attributes.InitiativeMod = numbers[0];
							}
							else if (identifier == "Speed")
							{
								char[] commaChar = { ',' };

								foreach (string speedStr in entry.Split(commaChar, StringSplitOptions.RemoveEmptyEntries))
								{
									string speedPattern = @"\s?(?<Type>\D*)\s(?<Speed>\d+)\s*ft.\s*(\((?<Manouverability>\w+)\))?";
									Regex speedRegex = new Regex(speedPattern, RegexOptions.IgnoreCase);
									Match speedMatch = speedRegex.Match(speedStr);

									if (speedMatch.Success)
									{
										Types.Manouverability manouverability = Types.Manouverability.None;
										if (speedMatch.Groups["Manouverability"].Success)
										{
											manouverability = Methods.GetManouverabilityFromString(speedMatch.Groups["Manouverability"].Value);
										}
										int distance = Convert.ToInt32(speedMatch.Groups["Speed"].Value);
										string movementString = speedMatch.Groups["Type"].Value.Trim();
										if (movementString.Equals(""))
										{
											attributes.Speed.LandSpeed = distance;
										}
										else
										{
											Types.Movement movementType = Methods.GetMovementTypeFromString(movementString);
											attributes.Speed.Speeds.Add(new Model.Speed(distance, movementType, manouverability));
										}
									}
								}
							}
							else if (identifier == "Armor Class")
							{
								string acPattern = @"(?<AC>\d+)\s\(.*\),\s*touch\s*(?<TouchAC>\d+),\s*flat\-footed\s*(?<FFAC>\d+)";
								Regex acRegex = new Regex(acPattern, RegexOptions.IgnoreCase);
								Match acMatch = acRegex.Match(entry);

								if (acMatch.Success)
								{
									attributes.ArmorClass = Convert.ToInt32(acMatch.Groups["AC"].Value);
									attributes.TouchArmorClass = Convert.ToInt32(acMatch.Groups["TouchAC"].Value);
									attributes.FlatFootedArmorClass = Convert.ToInt32(acMatch.Groups["FFAC"].Value);
								}
							}
							else if (identifier == "Base Attack/Grapple")
							{
								attributes.BaseAttackBonus = numbers[0];
								attributes.GrappleModifier = numbers[1];
							}
							else if (identifier == "Attack" || identifier == "Full Attack")
							{
								string[] orStr = { "or" };
								foreach (string attackSetStr in entry.Split(orStr, StringSplitOptions.RemoveEmptyEntries))
								{
									Model.AttackSet attackSet = new Model.AttackSet
									{
										Name = identifier,
									};

									string attackPattern = @"(?<NumAttacks>\d+)?\s?(?<Name>(?!and\b)\b\D+)\s(?<AttackMod>[\+\-]\d+)\s(?<Type>\D+\s?\D*)\s\((?<Damage>[^\(]*)\)";
									Regex attackRegex = new Regex(attackPattern, RegexOptions.IgnoreCase);
									MatchCollection attackMatches = attackRegex.Matches(attackSetStr);
									foreach (Match attackMatch in attackMatches)
									{
										int numAttacks = 1;
										string name = attackMatch.Groups["Name"].Value;
										if (attackMatch.Groups["NumAttacks"].Value != "")
										{
											numAttacks = Convert.ToInt32(attackMatch.Groups["NumAttacks"].Value);
											PluralizationService ps = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
											name = ps.Singularize(name);
										}

										name = char.ToUpper(name[0]) + name.Substring(1);

										for (int i = 0; i < numAttacks; ++i)
										{
											Model.Attack attack = new Model.Attack();

											attack.Name = name;
											attack.Modifier = Convert.ToInt32(attackMatch.Groups["AttackMod"].Value);
											attack.Type = Methods.GetAttackTypeFromString(attackMatch.Groups["Type"].Value);

											string damageStr = attackMatch.Groups["Damage"].Value;
											string damagePattern = @"(?<NumDice>\d+)(?<Die>d\d+)(?<DamageMod>[\+\-]?\d*)\s?(?<DamageType>(?!plus\b)\b\w+)?";
											Regex damageRegex = new Regex(damagePattern, RegexOptions.IgnoreCase);
											MatchCollection damageMatches = damageRegex.Matches(damageStr);

											foreach (Match damageMatch in damageMatches)
											{
												Model.Damage damage = new Model.Damage();
												damage.NumDice = Convert.ToInt32(damageMatch.Groups["NumDice"].Value);
												damage.Die = Methods.GetDieTypeFromString(damageMatch.Groups["Die"].Value);
												if (damageMatch.Groups["DamageMod"].Value != "")
												{
													damage.Modifier = Convert.ToInt32(damageMatch.Groups["DamageMod"].Value);
												}
												if (damageMatch.Groups["DamageType"].Value != "")
												{
													damage.DamageDescriptorSet.Add(Methods.GetDamageTypeFromString(damageMatch.Groups["DamageType"].Value));
												}
												attack.Damages.Add(damage);
											}

											attackSet.Attacks.Add(attack);
										}
									}

									attributes.AttackSets.Add(attackSet);
								}
							}
							else if (identifier == "Space/Reach")
							{
								attributes.Space = numbers[0];
								attributes.Reach = numbers[1];
							}
							else if (identifier == "Special Attacks")
							{
								attributes.SpecialAttacks = entry.Trim();
							}
							else if (identifier == "Special Qualities")
							{
								attributes.SpecialQualities = entry.Trim();

								string drPattern = @"damage reduction (?<Value>\d+)\/(?<Types>.+?)(\,|\z)";
								Regex drRegex = new Regex(drPattern, RegexOptions.IgnoreCase);
								MatchCollection drMatches = drRegex.Matches(entry);

								foreach (Match drMatch in drMatches)
								{
									Model.DamageReduction dr = new Model.DamageReduction();
									dr.Value = Convert.ToInt32(drMatch.Groups["Value"].Value);
									dr.DamageTypes = GetDamageDescriptorSetFromString(drMatch.Groups["Types"].Value, "and");
									attributes.DamageReductions.Add(dr);
								}

								string immunityPattern = @"immunity to (?<Types>.+?)(\,|\z)";
								Regex immunityRegex = new Regex(immunityPattern, RegexOptions.IgnoreCase);
								MatchCollection immunityMatches = immunityRegex.Matches(entry);

								foreach (Match immunityMatch in immunityMatches)
								{
									Model.DamageDescriptorSet damageTypes = GetDamageDescriptorSetFromString(immunityMatch.Groups["Types"].Value, "and");
									foreach (Types.Damage damageType in damageTypes.ToList())
									{
										attributes.Immunities.Add(damageType);
									}
								}

								string resistancesPattern = @"resistance to (?<Types>.+?)(\,|\z)";
								Regex resistancesRegex = new Regex(resistancesPattern, RegexOptions.IgnoreCase);
								Match resistancesMatch = resistancesRegex.Match(entry);

								if (resistancesMatch.Success)
								{
									string resistancePattern = @"(?<Type>[a-z]+)\s(?<Value>\d+)";
									Regex resistanceRegex = new Regex(resistancePattern, RegexOptions.IgnoreCase);
									MatchCollection resistanceMatches = resistanceRegex.Matches(resistancesMatch.Groups["Types"].Value);

									foreach (Match resistanceMatch in resistanceMatches)
									{
										Model.EnergyResistance res = new Model.EnergyResistance();
										res.Value = Convert.ToInt32(resistanceMatch.Groups["Value"].Value);
										res.EnergyType = Methods.GetDamageTypeFromString(resistanceMatch.Groups["Type"].Value);
										attributes.EnergyResistances.Add(res);
									}
								}

								string spellResistancePattern = @"spell resistance (?<Value>\d+)(\,|\z)";
								Regex spellResistanceRegex = new Regex(spellResistancePattern, RegexOptions.IgnoreCase);
								Match spellResistanceMatch = spellResistanceRegex.Match(entry);

								if (spellResistanceMatch.Success)
								{
									attributes.SpellResistance = Convert.ToInt32(spellResistanceMatch.Groups["Value"].Value);
								}

								string regenerationPattern = @"regeneration (?<Value>\d+)(\,|\z)";
								Regex regenerationRegex = new Regex(regenerationPattern, RegexOptions.IgnoreCase);
								Match regenerationMatch = regenerationRegex.Match(entry);

								if (regenerationMatch.Success)
								{
									attributes.FastHealing = Convert.ToInt32(regenerationMatch.Groups["Value"].Value);
								}

								string fastHealingPattern = @"fast healing (?<Value>\d+)(\,|\z)";
								Regex fastHealingRegex = new Regex(fastHealingPattern, RegexOptions.IgnoreCase);
								Match fastHealingMatch = fastHealingRegex.Match(entry);

								if (fastHealingMatch.Success)
								{
									attributes.FastHealing = Convert.ToInt32(fastHealingMatch.Groups["Value"].Value);
								}
							}
							else if (identifier == "Saves")
							{
								attributes.FortitudeSave = numbers[0];
								attributes.ReflexSave = numbers[1];
								attributes.WillSave = numbers[2];
							}
							else if (identifier == "Abilities")
							{
								string attributePattern = @"Str\s(?<Value>\w+)";
								Regex attributeRegex = new Regex(attributePattern, RegexOptions.IgnoreCase);
								Match attributeMatch = attributeRegex.Match(entry);

								if (attributeMatch.Success)
								{
									try
									{
										attributes.Strength = Convert.ToInt32(attributeMatch.Groups["Value"].Value);
									}
									catch (FormatException)
									{
										attributes.Strength = 0;
									}
								}

								attributePattern = @"Dex\s(?<Value>\w+)";
								attributeRegex = new Regex(attributePattern, RegexOptions.IgnoreCase);
								attributeMatch = attributeRegex.Match(entry);

								if (attributeMatch.Success)
								{
									try
									{
										attributes.Dexterity = Convert.ToInt32(attributeMatch.Groups["Value"].Value);
									}
									catch (FormatException)
									{
										attributes.Dexterity = 0;
									}
								}

								attributePattern = @"Con\s(?<Value>\w+)";
								attributeRegex = new Regex(attributePattern, RegexOptions.IgnoreCase);
								attributeMatch = attributeRegex.Match(entry);

								if (attributeMatch.Success)
								{
									try
									{
										attributes.Constitution = Convert.ToInt32(attributeMatch.Groups["Value"].Value);
									}
									catch (FormatException)
									{
										attributes.Constitution = 0;
									}
								}

								attributePattern = @"Int\s(?<Value>\w+)";
								attributeRegex = new Regex(attributePattern, RegexOptions.IgnoreCase);
								attributeMatch = attributeRegex.Match(entry);

								if (attributeMatch.Success)
								{
									try
									{
										attributes.Intelligence = Convert.ToInt32(attributeMatch.Groups["Value"].Value);
									}
									catch (FormatException)
									{
										attributes.Intelligence = 0;
									}
								}

								attributePattern = @"Wis\s(?<Value>\w+)";
								attributeRegex = new Regex(attributePattern, RegexOptions.IgnoreCase);
								attributeMatch = attributeRegex.Match(entry);

								if (attributeMatch.Success)
								{
									try
									{
										attributes.Wisdom = Convert.ToInt32(attributeMatch.Groups["Value"].Value);
									}
									catch (FormatException)
									{
										attributes.Wisdom = 0;
									}
								}

								attributePattern = @"Cha\s(?<Value>\w+)";
								attributeRegex = new Regex(attributePattern, RegexOptions.IgnoreCase);
								attributeMatch = attributeRegex.Match(entry);

								if (attributeMatch.Success)
								{
									try
									{
										attributes.Charisma = Convert.ToInt32(attributeMatch.Groups["Value"].Value);
									}
									catch (FormatException)
									{
										attributes.Charisma = 0;
									}
								}
							}
							else if (identifier == "Feats")
							{
								foreach (string feat in entry.Split(','))
								{
									attributes.Feats.Add(feat.Trim());
								}

								if (attributes.WeaponFinesse)
								{
									foreach (Model.AttackSet attackSet in attributes.AttackSets)
									{
										foreach (Model.Attack attack in attackSet.Attacks)
										{
											attack.Ability = Types.Ability.Dexterity;
										}
									}
								}
							}
							else if (identifier == "Challenge Rating")
							{
								if (numbers.Count == 2)
								{
									attributes.ChallengeRating = numbers[0] / numbers[1];
								}
								else
								{
									attributes.ChallengeRating = numbers[0];
								}
							}
						}
					}
				}
				catch (FormatException e)
				{
					MessageBox.Show("Cannot parse:\n" + currentLine);
					throw e;
				}
			}

			Model.Creature creature = new Model.Creature(attributes);

			return creature;
		}

		private static Model.DamageDescriptorSet GetDamageDescriptorSetFromString(string str, string separator = null)
		{
			string separatorStr = separator != null ? @"(?!" + separator + @"\b)\b" : separator;
			string typePattern = separatorStr + @"[a-z]+";	
			Regex typeRegex = new Regex(typePattern, RegexOptions.IgnoreCase);
			MatchCollection typeMatches = typeRegex.Matches(str);
			Model.DamageDescriptorSet damageDescriptorSet = new Model.DamageDescriptorSet();

			foreach (Match typeMatch in typeMatches)
			{
				try
				{
					damageDescriptorSet.Add(Methods.GetDamageTypeFromString(typeMatch.Value));
				}
				catch (FormatException)
				{
					// Format not recognised, never mind
				}
			}

			return damageDescriptorSet;
		}

		private static Model.Attack GetAttack(string name, int attackMod, Types.Attack attackType, List<Model.Damage> damages)
		{
			Model.Attack attack = new Model.Attack();
			attack.Name = name;
			attack.Modifier = attackMod;
			attack.Type = attackType;
			foreach (Model.Damage damage in damages)
			{
				attack.Damages.Add(damage);
			}
			return attack;
		}

		private static Model.Damage GetDamage(int numDice, Types.Die die, int damageMod, string[] damageTypeStrings)
		{
			Model.Damage damage = new Model.Damage();
			damage.NumDice = numDice;
			damage.Die = die;
			damage.Modifier = damageMod;
			foreach (string damageTypeString in damageTypeStrings)
			{
				damage.DamageDescriptorSet.Add(Methods.GetDamageTypeFromString(damageTypeString));
			}
			
			return damage;
		}
	}
}
