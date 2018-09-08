﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Forms;
using System.Xml;
using Dungeoneer.Utility;

namespace Dungeoneer.ViewModel
{
	public abstract class ActorInitiativeViewModel : BaseViewModel
	{
		public ActorInitiativeViewModel()
		{
			_backgroundColor = Colors.LightGray;
		}

		protected Model.Actor _actor;
		private string _displayName;
		private Color _backgroundColor;

		public Model.Actor Actor
		{
			get { return _actor; }
			set
			{
				_actor = value;
				NotifyPropertyChanged("Actor");
			}
		}

		public string DisplayName
		{
			get { return _displayName; }
			set
			{
				_displayName = value;
				NotifyPropertyChanged("DisplayName");
			}
		}

		public string ActorName
		{
			get { return Actor.ActorName; }
			set
			{
				Actor.ActorName = value;
				NotifyPropertyChanged("ActorName");
			}
		}

		public int InitiativeMod
		{
			get { return Actor.InitiativeMod; }
			set
			{
				Actor.InitiativeMod = value;
				NotifyPropertyChanged("InitiativeMod");
			}
		}

		public bool Active
		{
			get { return Actor.Active; }
			set
			{
				Actor.Active = value;
				NotifyPropertyChanged("Active");
			}
		}

		public FullyObservableCollection<Model.Effect.Effect> Effects
		{
			get { return Actor.Effects; }
			set
			{
				Actor.Effects = value;
				NotifyPropertyChanged("Effects");
			}
		}

		public Color BackgroundColor
		{
			get { return _backgroundColor; }
			set
			{
				_backgroundColor = value;
				NotifyPropertyChanged("BackgroundColor");
			}
		}

		public void StartTurn()
		{
			for (int i = Effects.Count - 1; i >= 0; --i)
			{
				

				if (Effects[i] is Model.Effect.TimedEffect)
				{
					Model.Effect.TimedEffect tempEffect = Effects[i] as Model.Effect.TimedEffect;
					tempEffect.ElapsedDuration++;
					if (tempEffect.ElapsedDuration == tempEffect.Duration)
					{
						Effects.RemoveAt(i);
					}
				}
			}
		}

		public void WriteXML(XmlWriter xmlWriter)
		{
			xmlWriter.WriteStartElement("ActorInitiativeViewModel");

			xmlWriter.WriteStartElement("DisplayName");
			xmlWriter.WriteString(DisplayName);
			xmlWriter.WriteEndElement();

			Actor.WriteXML(xmlWriter);

			xmlWriter.WriteEndElement();
		}

		public virtual void ReadXML(XmlNode xmlNode)
		{
			try
			{
				foreach (XmlNode childNode in xmlNode.ChildNodes)
				{
					if (childNode.Name == "DisplayName")
					{
						DisplayName = childNode.InnerText;
					}
					else
					{
						Model.Actor actor = new Model.Actor();
						actor.ReadXML(xmlNode);
						Actor = actor;
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
