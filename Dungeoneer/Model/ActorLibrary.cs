﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeoneer.Model
{
	public class ActorLibrary
	{
		public ActorLibrary()
		{
			LoadValues();
		}

		public ObservableCollection<Model.PlayerActor> Characters
		{
			get;
			set;
		}

		public ObservableCollection<Model.NonPlayerActor> Enemies
		{
			get;
			set;
		}

		public void LoadValues()
		{
			ObservableCollection<Model.PlayerActor> characters = new ObservableCollection<Model.PlayerActor>
			{
				new Model.PlayerActor { DisplayName = "Kolnik", ActorName = "Kolnik", InitiativeMod = -1 },
				new Model.PlayerActor { DisplayName = "Atrion", ActorName = "Atrion", InitiativeMod = 5 },
				new Model.PlayerActor { DisplayName = "Thrasin", ActorName = "Thrasin", InitiativeMod = 7 },
				new Model.PlayerActor { DisplayName = "Joshua", ActorName = "Joshua", InitiativeMod = 10 },
				new Model.PlayerActor { DisplayName = "Osprey", ActorName = "Osprey", InitiativeMod = 13 }
		};

			Characters = characters;

			ObservableCollection<Model.NonPlayerActor> enemies = new ObservableCollection<Model.NonPlayerActor>
			{
				new Model.Creature { ActorName = "Orc", InitiativeMod = 3, ChallengeRating = 0.5f, ArmourClass = 13, HitPoints = 8 },
				new Model.Creature { ActorName = "Goblin", InitiativeMod = 2, ChallengeRating = 0.25f, ArmourClass = 11, HitPoints = 6 },
				new Model.NonPlayerActor { ActorName = "Poison Dart Trap", InitiativeMod = 6, ChallengeRating = 4 }
			};

			Enemies = enemies;
		}
	}
}
