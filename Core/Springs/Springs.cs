using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezutils.Core
{
    public static class Springs
    {
		/// <summary>
		/// Calculate the spring motion for a given deltaTime. 
		/// Doenst account for edge cases for dampingRatio or angularFrequency 
		/// </summary>
		/// <param name="position"> Current position. </param>
		/// <param name="velocity"> Current velocity. </param>
		/// <param name="equilibriumPosition"> Rest position.</param>
		/// <param name="deltaTime"> Time to update over.</param>
		/// <param name="angularFrequency"> Angular frequency.</param>
		/// <param name="dampingRatio"> Damping ratio.</param>
		public static void UpdateDampedSHMFast(ref float position, ref float velocity,
				float equilibriumPosition, float deltaTime, float angularFrequency, float dampingRatio)
		{
			float displacement = position - equilibriumPosition;
			velocity += (-dampingRatio * velocity) - (angularFrequency * displacement);
			position += velocity * deltaTime;
		}

		/// <summary>
		/// Calculate the spring motion for a given deltaTime. 
		/// Doenst account for edge cases for dampingRatio or angularFrequency 
		/// </summary>
		/// <param name="position"> Current position. </param>
		/// <param name="velocity"> Current velocity. </param>
		/// <param name="equilibriumPosition"> Rest position.</param>
		/// <param name="deltaTime"> Time to update over.</param>
		/// <param name="angularFrequency"> Angular frequency.</param>
		/// <param name="dampingRatio"> Damping ratio.</param>
		public static void UpdateDampedSHMFast(ref Vector2 position, ref Vector2 velocity,
				Vector2 equilibriumPosition, float deltaTime, float angularFrequency, float dampingRatio)
		{
			Vector2 displacement = position - equilibriumPosition;
			velocity += (-dampingRatio * velocity) - (angularFrequency * displacement);
			position += velocity * deltaTime;
		}

		/// <summary>
		/// Calculate the spring motion for a given deltaTime. 
		/// Doenst account for edge cases for dampingRatio or angularFrequency 
		/// </summary>
		/// <param name="position"> Current position. </param>
		/// <param name="velocity"> Current velocity. </param>
		/// <param name="equilibriumPosition"> Rest position.</param>
		/// <param name="deltaTime"> Time to update over.</param>
		/// <param name="angularFrequency"> Angular frequency.</param>
		/// <param name="dampingRatio"> Damping ratio.</param>
		public static void UpdateDampedSHMFast(ref Vector3 position, ref Vector3 velocity,
				Vector3 equilibriumPosition, float deltaTime, float angularFrequency, float dampingRatio)
		{
			Vector3 displacement = position - equilibriumPosition;
			velocity += (-dampingRatio * velocity) - (angularFrequency * displacement);
			position += velocity * deltaTime;
		}
	}
}
