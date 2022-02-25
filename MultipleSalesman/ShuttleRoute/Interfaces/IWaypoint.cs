﻿namespace MultipleSalesman
{
    internal interface IWaypoint<T>
    {
        public double GetCost(T waypoint);

        public bool AreEqual(T waypoint);

        public T GetValue();
    }
}
