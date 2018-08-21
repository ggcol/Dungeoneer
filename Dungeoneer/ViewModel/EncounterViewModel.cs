﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeoneer.ViewModel
{
	public class EncounterViewModel : BaseViewModel
	{
		private Utility.FullyObservableCollection<InitiativeCardViewModel> _initiativeTrack;
		private int _round;
		private RelayCommand _nextRound;
		private Command _save;

		public EncounterViewModel()
		{
			_initiativeTrack = new Utility.FullyObservableCollection<InitiativeCardViewModel>();
			_round = 1;
			_nextRound = new RelayCommand(ExecuteNextRound, CheckRound);
			_save = new Command(ExecuteSave);
		}

		public int Round
		{
			get { return _round; }
			set
			{
				_round = value;
				NotifyPropertyChanged("Round");
			}
		}

		public RelayCommand NextRound
		{
			get { return _nextRound; }
		}

		private void ExecuteNextRound()
		{
			++Round;

			foreach (InitiativeCardViewModel initCard in _initiativeTrack)
			{
				initCard.TurnEnded = false;
			}
		}

		private bool CheckRound()
		{
			bool roundComplete = true;
			foreach (InitiativeCardViewModel initCard in _initiativeTrack)
			{
				if (!initCard.TurnEnded && !initCard.Delayed && !initCard.Readied)
				{
					roundComplete = false;
				}
			}

			return roundComplete;
		}
		
		public Utility.FullyObservableCollection<InitiativeCardViewModel> InitiativeTrack
		{
			get { return _initiativeTrack; }
			set
			{
				_initiativeTrack = value;
				NotifyPropertyChanged("InitiativeTrack");
			}
		}

		public void AddActor(Model.Actor actor)
		{
			ActorViewModel actorViewModel = ActorViewModelFactory.GetActorViewModel(actor);
			InitiativeTrack.Add( new InitiativeCardViewModel() { ActorViewModel = actorViewModel } );
		}

		public void AddInitiativeCard(InitiativeCardViewModel initCard)
		{
			InitiativeTrack.Add(initCard);
		}

		public int GetNumberOfActorsWithName(string actorName)
		{
			int count = 0;
			foreach (InitiativeCardViewModel initCard in InitiativeTrack)
			{
				if (initCard.ActorViewModel.ActorName == actorName)
				{
					++count;
				}
			}
			return count;
		}

		public void ExecuteSave()
		{

		}
	}
}
