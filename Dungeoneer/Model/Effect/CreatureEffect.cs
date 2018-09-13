﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeoneer.Model.Effect
{
	public abstract class CreatureEffect : Effect
	{
		public CreatureEffect(bool perTurn)
			: base(perTurn)
		{

		}

		public override Actor ApplyTo(Actor actor)
		{
			return ApplyTo(actor as Creature);
		}

		public abstract Creature ApplyTo(Creature creature);
	}
}