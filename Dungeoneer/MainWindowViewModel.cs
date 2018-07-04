﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeoneer
{
	class MainWindowViewModel
	{
		public MainWindowViewModel()
		{
			LoadValues();
		}

		public ObservableCollection<Actor> Characters
		{
			get;
			set;
		}

		public void LoadValues()
		{
			ObservableCollection<Actor> characters = new ObservableCollection<Actor>
			{
				new Actor { Name = "Kolnik", InitiativeMod = new NamedValue { Name = "Init", Value = -1 }, },
				new Actor { Name = "Atrion", InitiativeMod = new NamedValue { Name = "Init", Value = 5 }, },
				new Actor { Name = "Thrasin", InitiativeMod = new NamedValue { Name = "Init", Value = 7 }, },
				new Actor { Name = "Joshua", InitiativeMod = new NamedValue { Name = "Init", Value = 10 }, }
			};

			Characters = characters;
		}

		public ObservableCollection<InitiativeItem> InitiativeTrack
		{
			get;
			set;
		}

		
	}
}