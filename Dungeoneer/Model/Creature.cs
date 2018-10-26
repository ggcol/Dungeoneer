﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using Dungeoneer.Utility;
using System.Collections.ObjectModel;

namespace Dungeoneer.Model
{
	public class Creature : NonPlayerActor
	{
		public Creature()
			: base()
		{
			Strength = 10;
			Dexterity = 10;
			Constitution = 10;
			Intelligence = 10;
			Wisdom = 10;
			Charisma = 10;
			BaseAttackBonus = 0;
			GrappleModifier = 0;
			HitPoints = 3;
			HitDice = 1;
			HitDieType = Types.Die.d3;
			ArmorClass = 10;
			TouchArmorClass = 10;
			FlatFootedArmorClass = 10;
			Speed = 30;
			FortitudeSave = 0;
			ReflexSave = 0;
			WillSave = 0;
			Feats = new List<string>();
			Space = 5;
			Reach = 5;
			Size = Types.Size.Medium;
			DamageReductions = new ObservableCollection<DamageReduction>();
			Immunities = new DamageDescriptorSet();
		}

		public Creature(Creature creature)
			: base()
		{
			Strength = creature.Strength;
			Dexterity = creature.Dexterity;
			Constitution = creature.Constitution;
			Intelligence = creature.Intelligence;
			Wisdom = creature.Wisdom;
			Charisma = creature.Charisma;
			BaseAttackBonus = creature.BaseAttackBonus;
			GrappleModifier = creature.GrappleModifier;
			HitPoints = creature.HitPoints;
			HitDice = creature.HitDice;
			HitDieType = creature.HitDieType;
			ArmorClass = creature.ArmorClass;
			TouchArmorClass = creature.TouchArmorClass;
			FlatFootedArmorClass = creature.FlatFootedArmorClass;
			Speed = creature.Speed;
			FortitudeSave = creature.FortitudeSave;
			ReflexSave = creature.ReflexSave;
			WillSave = creature.WillSave;
			Feats = creature.Feats;
			Space = creature.Space;
			Reach = creature.Reach;
			Size = creature.Size;
			DamageReductions = creature.DamageReductions;
			Immunities = creature.Immunities;
		}

		private int _strength;
		private int _dexterity;
		private int _constitution;
		private int _intelligence;
		private int _wisdom;
		private int _charisma;

		private int _baseAttackBonus;
		private int _grappleModifier;
		private int _hitPoints;
		private int _hitDice;
		private Types.Die _hitDieType;

		private int _armorClass;
		private int _touchArmorClass;
		private int _flatFootedArmorClass;

		private int _speed;

		private int _fortitudeSave;
		private int _reflexSave;
		private int _willSave;

		private List<string> _feats;

		private int _space;
		private int _reach;
		private Types.Size _size;
		private ObservableCollection<DamageReduction> _damageReductions;
		private DamageDescriptorSet _immunities;

		public int GetAbilityScore(Types.Ability ability)
		{
			switch (ability)
			{
			case Types.Ability.Strength:
				return Strength;
			case Types.Ability.Dexterity:
				return Dexterity;
			case Types.Ability.Constitution:
				return Constitution;
			case Types.Ability.Intelligence:
				return Intelligence;
			case Types.Ability.Wisdom:
				return Wisdom;
			case Types.Ability.Charisma:
				return Charisma;
			}
			return 0;
		}

		public int GetAbilityModifier(Types.Ability ability)
		{
			return Methods.GetAbilityModifier(GetAbilityScore(ability));
		}

		public void SetAbilityScore(Types.Ability ability, int score)
		{
			switch (ability)
			{
			case Types.Ability.Strength:
				Strength = score;
				break;
			case Types.Ability.Dexterity:
				Dexterity = score;
				break;
			case Types.Ability.Constitution:
				Constitution = score;
				break;
			case Types.Ability.Intelligence:
				Intelligence = score;
				break;
			case Types.Ability.Wisdom:
				Wisdom = score;
				break;
			case Types.Ability.Charisma:
				Charisma = score;
				break;
			}
		}

		private Creature GetAffectedCreature()
		{
			Creature temp = new Creature(this);
			foreach (Effect.Effect effect in Effects)
			{
				effect.ApplyTo(ref temp);
			}
			return temp;
		}

		public int Strength
		{
			get
			{
				return GetAffectedCreature()._strength;
			}
			set
			{
				_strength = value;
				NotifyPropertyChanged("Strength");
			}
		}

		public int Dexterity
		{
			get { return GetAffectedCreature()._dexterity; }
			set
			{
				_dexterity = value;
				NotifyPropertyChanged("Dexterity");
			}
		}

		public int Constitution
		{
			get { return GetAffectedCreature()._constitution; }
			set
			{
				_constitution = value;
				NotifyPropertyChanged("Constitution");
			}
		}

		public int Intelligence
		{
			get { return GetAffectedCreature()._intelligence; }
			set
			{
				_intelligence = value;
				NotifyPropertyChanged("Intelligence");
			}
		}

		public int Wisdom
		{
			get { return GetAffectedCreature()._wisdom; }
			set
			{
				_wisdom = value;
				NotifyPropertyChanged("Wisdom");
			}
		}

		public int Charisma
		{
			get { return GetAffectedCreature()._charisma; }
			set
			{
				_charisma = value;
				NotifyPropertyChanged("Charisma");
			}
		}

		public int BaseAttackBonus
		{
			get { return GetAffectedCreature()._baseAttackBonus; }
			set
			{
				_baseAttackBonus = value;
				NotifyPropertyChanged("BaseAttackBonus");
			}
		}

		public int GrappleModifier
		{
			get { return GetAffectedCreature()._grappleModifier; }
			set
			{
				_grappleModifier = value;
				NotifyPropertyChanged("BaseGrappleModifierAttackBonus");
			}
		}

		public int HitPoints
		{
			get { return GetAffectedCreature()._hitPoints; }
			set
			{
				_hitPoints = value;
				NotifyPropertyChanged("HitPoints");
			}
		}

		public int HitDice
		{
			get { return GetAffectedCreature()._hitDice; }
			set
			{
				_hitDice = value;
				NotifyPropertyChanged("HitDice");
			}
		}

		public Types.Die HitDieType
		{
			get { return GetAffectedCreature()._hitDieType; }
			set
			{
				_hitDieType = value;
				NotifyPropertyChanged("HitDiceType");
			}
		}

		public int ArmorClass
		{
			get { return GetAffectedCreature()._armorClass; }
			set
			{
				_armorClass = value;
				NotifyPropertyChanged("ArmorClass");
			}
		}

		public int TouchArmorClass
		{
			get { return GetAffectedCreature()._touchArmorClass; }
			set
			{
				_touchArmorClass = value;
				NotifyPropertyChanged("TouchArmorClass");
			}
		}

		public int FlatFootedArmorClass
		{
			get { return GetAffectedCreature()._flatFootedArmorClass; }
			set
			{
				_flatFootedArmorClass = value;
				NotifyPropertyChanged("FlatFootedArmorClass");
			}
		}

		public int Speed
		{
			get { return GetAffectedCreature()._speed; }
			set
			{
				_speed = value;
				NotifyPropertyChanged("Speed");
			}
		}

		public int FortitudeSave
		{
			get { return GetAffectedCreature()._fortitudeSave; }
			set
			{
				_fortitudeSave = value;
				NotifyPropertyChanged("FortitudeSave");
			}
		}

		public int ReflexSave
		{
			get { return GetAffectedCreature()._reflexSave; }
			set
			{
				_reflexSave = value;
				NotifyPropertyChanged("ReflexSave");
			}
		}

		public int WillSave
		{
			get { return GetAffectedCreature()._willSave; }
			set
			{
				_willSave = value;
				NotifyPropertyChanged("WillSave");
			}
		}

		public bool PowerAttack
		{
			get
			{
				return Feats.Contains("Power Attack", StringComparer.CurrentCultureIgnoreCase) &&
					Strength >= 13;
			}
		}

		public int Space
		{
			get { return GetAffectedCreature()._space; }
			set
			{
				_space = value;
				NotifyPropertyChanged("Space");
			}
		}

		public int Reach
		{
			get { return GetAffectedCreature()._reach; }
			set
			{
				_reach = value;
				NotifyPropertyChanged("Reach");
			}
		}

		public List<string> Feats
		{
			get { return _feats; }
			set
			{
				_feats = value;
				NotifyPropertyChanged("Feats");
				NotifyPropertyChanged("PowerAttack");
			}
		}

		public Types.Size Size
		{
			get { return GetAffectedCreature()._size; }
			set
			{
				_size = value;
				NotifyPropertyChanged("Size");
			}
		}
		
		public ObservableCollection<DamageReduction> DamageReductions
		{
			get { return GetAffectedCreature()._damageReductions; }
			set
			{
				_damageReductions = value;
				NotifyPropertyChanged("DamageReductions");
			}
		}

		public DamageDescriptorSet Immunities
		{
			get { return GetAffectedCreature()._immunities; }
			set
			{
				_immunities = value;
				NotifyPropertyChanged("Immunities");
			}
		}

		public override void WriteXMLStartElement(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("Creature");
		}

		public override void WritePropertyXML(XmlWriter xmlWriter)
		{
			base.WritePropertyXML(xmlWriter);

			xmlWriter.WriteStartElement("Strength");
			xmlWriter.WriteString(Strength.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Dexterity");
			xmlWriter.WriteString(Dexterity.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Constitution");
			xmlWriter.WriteString(Constitution.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Intelligence");
			xmlWriter.WriteString(Intelligence.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Wisdom");
			xmlWriter.WriteString(Wisdom.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Charisma");
			xmlWriter.WriteString(Charisma.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("BaseAttackBonus");
			xmlWriter.WriteString(BaseAttackBonus.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("GrappleModifier");
			xmlWriter.WriteString(GrappleModifier.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("HitPoints");
			xmlWriter.WriteString(HitPoints.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("HitDice");
			xmlWriter.WriteString(HitDice.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("HitDieType");
			xmlWriter.WriteString(Methods.GetDieTypeString(HitDieType));
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("ArmorClass");
			xmlWriter.WriteString(ArmorClass.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("TouchArmorClass");
			xmlWriter.WriteString(TouchArmorClass.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("FlatFootedArmorClass");
			xmlWriter.WriteString(FlatFootedArmorClass.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Speed");
			xmlWriter.WriteString(Speed.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("FortitudeSave");
			xmlWriter.WriteString(FortitudeSave.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("ReflexSave");
			xmlWriter.WriteString(ReflexSave.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("WillSave");
			xmlWriter.WriteString(WillSave.ToString());
			xmlWriter.WriteEndElement();
			
			xmlWriter.WriteStartElement("Feats");
			foreach (string feat in Feats)
			{
				xmlWriter.WriteStartElement("Feat");
				xmlWriter.WriteString(feat);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Space");
			xmlWriter.WriteString(Space.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Reach");
			xmlWriter.WriteString(Reach.ToString());
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Size");
			xmlWriter.WriteString(Methods.GetSizeString(Size));
			xmlWriter.WriteEndElement();
			
			xmlWriter.WriteStartElement("DamageReductions");
			foreach (DamageReduction dr in DamageReductions)
			{
				dr.WriteXML(xmlWriter);
			}
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Immunities");
			Immunities.WriteXML(xmlWriter);
			xmlWriter.WriteEndElement();
		}

		public override void ReadXML(XmlNode xmlNode)
		{
			base.ReadXML(xmlNode);

			try
			{
				foreach (XmlNode childNode in xmlNode.ChildNodes)
				{
					if (childNode.Name == "Strength")
					{
						Strength = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "Dexterity")
					{
						Dexterity = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "Constitution")
					{
						Constitution = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "Intelligence")
					{
						Intelligence = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "Wisdom")
					{
						Wisdom = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "Charisma")
					{
						Charisma = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "BaseAttackBonus")
					{
						BaseAttackBonus = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "GrappleModifier")
					{
						GrappleModifier = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "HitPoints")
					{
						HitPoints = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "HitDice")
					{
						HitDice = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "HitDieType")
					{
						HitDieType = Methods.GetDieTypeFromString(childNode.InnerText);
					}
					else if (childNode.Name == "ArmorClass")
					{
						ArmorClass = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "TouchArmorClass")
					{
						TouchArmorClass = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "FlatFootedArmorClass")
					{
						FlatFootedArmorClass = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "Speed")
					{
						Speed = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "FortitudeSave")
					{
						FortitudeSave = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "ReflexSave")
					{
						ReflexSave = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "WillSave")
					{
						WillSave = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "Feats")
					{
						foreach (XmlNode featNode in childNode.ChildNodes)
						{
							if (featNode.Name == "Feat")
							{
								Feats.Add(featNode.InnerText);
							}
						}
					}
					else if (childNode.Name == "Space")
					{
						Space = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "Reach")
					{
						Reach = Convert.ToInt32(childNode.InnerText);
					}
					else if (childNode.Name == "Size")
					{
						Size = Methods.GetSizeFromString(childNode.InnerText);
					}
					else if (childNode.Name == "DamageReductions")
					{
						foreach (XmlNode drNode in childNode.ChildNodes)
						{
							if (drNode.Name == "DamageReduction")
							{
								DamageReduction dr = new DamageReduction();
								dr.ReadXML(drNode);
								DamageReductions.Add(dr);
							}
						}
					}
				}
			}
			catch (XmlException e)
			{
				MessageBox.Show(e.ToString());
			}
		}
	}
}
