﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dungeoneer.ViewModel
{
	public class AttackViewModel : BaseViewModel
	{
		public AttackViewModel()
		{
			_attack = new Model.Attack();
		}

		private Model.Attack _attack;

		public Model.Attack Attack
		{
			get { return _attack; }
			set
			{
				_attack = value;
				NotifyPropertyChanged("Attack");
			}
		}

		public string Name
		{
			get { return Attack.Name; }
			set
			{
				Attack.Name = value;
				NotifyPropertyChanged("Name");
			}
		}

		public string AttackMod
		{
			get
			{
				return Utility.Methods.GetSignedNumberString(Attack.AttackMod);
			}
			set
			{
				Attack.AttackMod = Convert.ToInt32(value);
				NotifyPropertyChanged("AttackMod");
			}
		}

		public string AttackType
		{
			get
			{
				return Utility.Methods.GetAttackTypeString(Attack.AttackType);
			}
			set
			{
				Attack.AttackType = Utility.Methods.GetAttackTypeFromString(value);
				NotifyPropertyChanged("AttackType");
			}
		}

		public string ThreatRange
		{
			get
			{
				return Utility.Methods.GetThreatRangeString(Attack.ThreatRangeMin);
			}
			set
			{
				string min = value.Substring(0, 2);
				Attack.ThreatRangeMin = Convert.ToInt32(min);
				NotifyPropertyChanged("ThreatRange");
			}
		}

		public string CritMultiplier
		{
			get
			{
				return "x" + Convert.ToString(Attack.CritMultiplier);
			}
			set
			{
				string multiplier = value.Substring(1, 1);
				Attack.CritMultiplier = Convert.ToInt32(multiplier);
				NotifyPropertyChanged("CritMultiplier");
			}
		}

		public string Damages
		{
			get
			{
				return Utility.Methods.GetDamageString(Attack.Damages);
			}
		}

		public string AttackAndDamage
		{
			get
			{
				return AttackMod + " " + Damages;
			}
		}

		public void WriteXML(XmlWriter xmlWriter)
		{
			Attack.WriteXML(xmlWriter);
		}

		public void ReadXML(XmlNode xmlNode)
		{
			Model.Attack attack = new Model.Attack();
			attack.ReadXML(xmlNode);
			Attack = attack;
		}
	}
}
