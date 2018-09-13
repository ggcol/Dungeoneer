﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using Dungeoneer.Utility;

namespace Dungeoneer.ViewModel
{
	public class CreatureInitiativeViewModel : NonPlayerActorInitiativeViewModel
	{
		public CreatureInitiativeViewModel(EncounterViewModel encounterViewModel)
		{
			encounterViewModel.OnWeaponListChange += OnWeaponListChange;
			_doDamage = new Command(ExecuteDoDamage);
			_actor = new Model.Creature();
			_weaponList = encounterViewModel.WeaponList;
			_showEffectsWindow = new Command(ExecuteShowEffectsWindow);
			_hideEffectsWindow = new Command(ExecuteHideEffectsWindow);
		}

		private FullyObservableCollection<Model.WeaponSet> _weaponList;
		private Command _doDamage;
		private View.EffectsWindow _effectsWindow;
		private Command _showEffectsWindow;
		private Command _hideEffectsWindow;

		public void OnWeaponListChange(FullyObservableCollection<Model.WeaponSet> weaponList)
		{
			WeaponList = weaponList;
		}

		public FullyObservableCollection<Model.WeaponSet> WeaponList
		{
			get { return _weaponList; }
			set
			{
				_weaponList = value;
				NotifyPropertyChanged("WeaponList");
			}
		}

		public new Model.Creature Actor
		{
			get { return _actor as Model.Creature; }
			set
			{
				_actor = value;
				ActorUpdated();
			}
		}

		public string ArmourClass
		{
			get { return Actor.ArmourClass.ToString(); }
			set
			{
				Actor.ArmourClass = Convert.ToInt32(value);
				ArmourClassUpdated();
			}
		}

		public string HitPoints
		{
			get { return Actor.HitPoints.ToString(); }
			set
			{
				Actor.HitPoints = Convert.ToInt32(value);
				HitPointsUpdated();
			}
		}

		private void ActorUpdated()
		{
			NotifyPropertyChanged("Actor");
			ArmourClassUpdated();
			HitPointsUpdated();
		}

		private void ArmourClassUpdated()
		{
			NotifyPropertyChanged("ArmourClass");
		}

		private void HitPointsUpdated()
		{
			NotifyPropertyChanged("HitPoints");
			Active = (Actor.HitPoints > 0);
			BackgroundColor = Active ? Colors.LightGray : Colors.DarkRed;
		}

		public override void StartTurn()
		{
			foreach (Model.Effect.Effect effect in Effects)
			{
				if (effect.PerTurn && effect is Model.Effect.CreatureEffect)
				{
					Actor = (effect as Model.Effect.CreatureEffect).ApplyTo(Actor);
				}
			}

			base.StartTurn();
		}

		private Model.Creature GetAffectedActor()
		{
			Model.Creature affectedActor = Actor;

			foreach (Model.Effect.Effect effect in Effects)
			{
				if (!effect.PerTurn && effect is Model.Effect.CreatureEffect)
				{
					affectedActor = (effect as Model.Effect.CreatureEffect).ApplyTo(affectedActor);
				}
			}

			return affectedActor;
		}

		public Command DoDamage
		{
			get { return _doDamage; }
		}

		private void ExecuteDoDamage()
		{
			DoDamageDialogViewModel doDamageDialogViewModel = new DoDamageDialogViewModel(WeaponList);
			Model.Creature creature = doDamageDialogViewModel.DoDamage(Actor, Effects);
			if (creature != null)
			{
				Actor = creature;
			}
		}

		public Command ShowEffectsWindow
		{
			get { return _showEffectsWindow; }
		}

		private void ExecuteShowEffectsWindow()
		{
			_effectsWindow = new View.EffectsWindow();
			_effectsWindow.DataContext = this;
			_effectsWindow.Show();
		}

		public Command HideEffectsWindow
		{
			get { return _hideEffectsWindow; }
		}

		private void ExecuteHideEffectsWindow()
		{
			_effectsWindow.Close();
		}

		public override void WriteXMLStartElement(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("CreatureInitiativeViewModel");
		}

		public override void WriteActorXML(XmlWriter xmlWriter)
		{
			Actor.WriteXML(xmlWriter);
		}

		public override void ReadActorXML(XmlNode xmlNode)
		{
			Model.Creature creature = new Model.Creature();
			creature.ReadXML(xmlNode);
			Actor = creature;
		}
	}
}
