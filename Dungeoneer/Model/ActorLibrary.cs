﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

namespace Dungeoneer.Model
{
	public class ActorLibrary : BaseModel
	{
		public ActorLibrary()
		{
			_characters = new ObservableCollection<PlayerActor>();
			_enemies = new ObservableCollection<Creature>();
			Load();
		}

		private ObservableCollection<PlayerActor> _characters;
		private ObservableCollection<Creature> _enemies;

		public ObservableCollection<PlayerActor> Characters
		{
			get { return _characters; }
			set
			{
				_characters = value;
				NotifyPropertyChanged("Characters");
			}
		}

		public ObservableCollection<Creature> Enemies
		{
			get { return _enemies; }
			set
			{
				_enemies = value;
				NotifyPropertyChanged("Enemies");
			}
		}

		public void Load()
		{
			if (!ReadXML())
			{
				LoadTestData();
			}
		}

		public void EditActor(Actor oldActor, Actor newActor)
		{
			if (oldActor is PlayerActor)
			{
				var foundCharacter = Characters.Where(character => character == oldActor).FirstOrDefault();

				if (foundCharacter != null)
				{
					Characters.Remove(oldActor as PlayerActor);
					Characters.Add(newActor as PlayerActor);
				}
			}
			else if (oldActor is Creature)
			{
				var foundEnemy = Enemies.Where(enemy => enemy == oldActor).FirstOrDefault();

				if (foundEnemy != null)
				{
					Enemies.Remove(oldActor as Creature);
					Enemies.Add(newActor as Creature);
				}
			}
		}
		
		public void LoadTestData()
		{
/*
			ObservableCollection<PlayerActor> characters = new ObservableCollection<PlayerActor>
			{
				new PlayerActor { ActorName = "Kolnik", InitiativeMod = -1 },
				new PlayerActor { ActorName = "Atrion", InitiativeMod = 5 },
				new PlayerActor { ActorName = "Thrasin", InitiativeMod = 7 },
				new PlayerActor { ActorName = "Joshua", InitiativeMod = 10 },
				new PlayerActor { ActorName = "Osprey", InitiativeMod = 13 }
			};

			Characters = characters;

			ObservableCollection<NonPlayerActor> enemies = new ObservableCollection<NonPlayerActor>
			{
				new Creature { ActorName = "Orc", InitiativeMod = 3, ChallengeRating = 0.5f, ArmorClass = 13, HitPoints = 8 },
				new Creature { ActorName = "Goblin", InitiativeMod = 2, ChallengeRating = 0.25f, ArmorClass = 11, HitPoints = 6 },
				new NonPlayerActor { ActorName = "Poison Dart Trap", InitiativeMod = 6, ChallengeRating = 4 },
				new Creature { ActorName = "Troll", ArmorClass = 16, HitPoints = 52 }
			};

			Enemies = enemies;
*/
		}

		public void WriteXML()
		{
			XmlWriter xmlWriter = XmlWriter.Create("ActorLibrary.xml");

			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement("ActorLibrary");

			xmlWriter.WriteStartElement("Characters");
			foreach (PlayerActor character in Characters)
			{
				character.WriteXML(xmlWriter);
			}
			xmlWriter.WriteEndElement();

			xmlWriter.WriteStartElement("Enemies");
			foreach (Creature enemy in Enemies)
			{
				enemy.WriteXML(xmlWriter);
			}
			xmlWriter.WriteEndElement();

			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
			xmlWriter.Close();
		}

		public bool ReadXML()
		{
			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load("ActorLibrary.xml");

				foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
				{
					if (xmlNode.Name == "Characters")
					{
						foreach (XmlNode characterNode in xmlNode.ChildNodes)
						{
							if (characterNode.Name == "PlayerActor")
							{
								Characters.Add(new PlayerActor(characterNode));
							}
						}
					}
					else if (xmlNode.Name == "Enemies")
					{
						foreach (XmlNode enemyNode in xmlNode.ChildNodes)
						{
							if (enemyNode.Name == "Creature")
							{
								Enemies.Add(new Creature(enemyNode));
							}
						}
					}
				}
			}
			catch (System.IO.FileNotFoundException)
			{
				return false;
			}
			catch (XmlException e)
			{
				MessageBox.Show(e.ToString());
				return false;
			}

			return true;
		}
	}
}
