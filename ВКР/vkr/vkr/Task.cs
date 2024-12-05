using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;

namespace vkr
{
    public class Method
    {
        private PointLatLng[][] elemPoints;
        private BaseStationType[] types;
        private PointLatLng[] baseStationPlaces;
        private double ha;
        private int[][] coverage;
        readonly Random rng = new Random();
        public int[][] Coverage { get { return coverage; } }
        public Method(PointLatLng[][] elemPoints, PointLatLng[] baseStations, BaseStationType[] types, float ha)
        {
            this.elemPoints = elemPoints;
            this.baseStationPlaces = baseStations;
            this.types = types;
            this.ha = ha;
        }

        public void Preparation(int territoryType)
        {
            double[][] distances = new double[baseStationPlaces.Length][];
            double[][] signals = new double[types.Length * baseStationPlaces.Length][];
            int[][] coverage = new int[types.Length * baseStationPlaces.Length][];
            for (int i = 0; i < baseStationPlaces.Length; i++) 
            {
                distances[i] = new double[elemPoints.Length * elemPoints[0].Length];
                for (int j = 0; j < types.Length; j++)
                {
                    signals[i * types.Length + j] = new double[elemPoints.Length * elemPoints[0].Length];
                    coverage[i * types.Length + j] = new int[elemPoints.Length * elemPoints[0].Length];
                }
            }

            for(int i = 0;i < baseStationPlaces.Length; i++)
            {
                PointLatLng bsPlace = baseStationPlaces[i];
                for (int k = 0; k < elemPoints.Length; k++)
                    for(int l = 0; l < elemPoints[0].Length; l++)
                        distances[i][k * elemPoints[0].Length + l] = DistanceInKmBetweenEarthCoordinates(
                            bsPlace.Lat, bsPlace.Lng, elemPoints[k][l].Lat, elemPoints[k][l].Lng);
            }

            for (int i = 0; i < baseStationPlaces.Length; i++)
                for (int j = 0; j < types.Length;j++) 
                    for (int k = 0; k < distances[0].Length; k++) 
                        signals[i * types.Length + j][k] = Signal(types[j], 1000 * distances[i][k], 0);

            for (int i = 0; i < signals.Length; i++)
                for (int k = 0; k < signals[0].Length; k++)
                    if (signals[i][k] >= 13)
                        coverage[i][k] = 1;
                    else
                        coverage[i][k] = 0;
            this.coverage = coverage;
        }

        public Individual GeneticAlgo(int generations, int maxPopulation)
        {
            var population = InitPopulation(maxPopulation);
           
            for (int i = 0; i <= generations; i++) 
            { 
                var child = new Individual
                {
                    Gens = new int[types.Length * baseStationPlaces.Length]
                };
                int parentI1 = rng.Next(0, population.Count - 1);
                int parentI2 = rng.Next(0, population.Count - 1);
                while (parentI1 == parentI2) 
                    parentI2 = rng.Next(0, population.Count - 1);
                var parent1 = population[parentI1];
                var parent2 = population[parentI2];
                child = Cross(child, parent1, parent2);
                child = Mutation(child);
                ReplaceWorst(child, population);
            }
            int bestIndividI = 0;
            for (int j = 0; j < population.Count; j++)
                if (population[j].Cost < population[bestIndividI].Cost)
                    bestIndividI = j;
            return population[bestIndividI];
        }

        List<Individual> InitPopulation(int maxPopulation)
        {
            List<Individual> population = new List<Individual>();
            while (population.Count < maxPopulation)
            {
                var ind = new Individual
                {
                    Gens = new int[types.Length * baseStationPlaces.Length]
                };
                for (int j = 0; j < baseStationPlaces.Length; j++)
                {
                    int typeI = rng.Next(-1, types.Length);
                    if (typeI != -1)
                        ind.Gens[j * types.Length + typeI] = 1;
                }
                if (CheckIndividual(ind))
                    population.Add(ind);
            }
            return population;
        }

        Individual Cross(Individual child, Individual parent1, Individual parent2) 
        {
            int crossPlace = rng.Next(0, baseStationPlaces.Length - 1);
            for (int j = 0; j < parent1.Gens.Length; j++)
                child.Gens[j] = parent1.Gens[j];
            if (rng.NextDouble() < 0.5)
                for (int j = 0; j < types.Length; j++)
                    child.Gens[crossPlace * types.Length + j] = parent1.Gens[crossPlace * types.Length + j];
            else
                for (int j = 0; j < types.Length; j++)
                    child.Gens[crossPlace * types.Length + j] = parent2.Gens[crossPlace * types.Length + j];
            return child;
        }

        Individual Mutation(Individual child)
        {
            Individual mutatedChild;
            do
            {
                mutatedChild = new Individual
                {
                    Gens = new int[types.Length * baseStationPlaces.Length]
                };
                for (int j = 0; j < child.Gens.Length; j++)
                    mutatedChild.Gens[j] = child.Gens[j];
                int mutationPlace = rng.Next(0, baseStationPlaces.Length);
                int mutationType = rng.Next(-1, types.Length);

                for (int j = 0; j < types.Length; j++)
                    mutatedChild.Gens[mutationPlace * types.Length + j] = 0;

                if (mutationType != -1)
                    mutatedChild.Gens[mutationPlace * types.Length + mutationType] = 1;
            }
            while (!CheckIndividual(mutatedChild));
            return mutatedChild;
        }

        void ReplaceWorst(Individual child, List<Individual> population)
        {
            int worstIndividI = 0;
            for (int j = 0; j < population.Count; j++)
                if ((population[j].covered < population[worstIndividI].covered) ||
                    (population[j].covered == population[worstIndividI].covered &&
                    population[j].Cost > population[worstIndividI].Cost))
                    worstIndividI = j;

            if ((child.covered > population[worstIndividI].covered) ||
                (child.covered == population[worstIndividI].covered &&
                child.Cost < population[worstIndividI].Cost))
                population[worstIndividI] = child;
        }

        public bool CheckIndividual(Individual ind)
        {
            ind.covered = 0;
            for (int j = 0; j < coverage[0].Length; j++)
            { 
                int isCovered = 0;
                for (int i = 0; i < ind.Gens.Length; i++)
                {
                    isCovered += coverage[i][j] * ind.Gens[i];
                }
                if (isCovered > 0)
                    ind.covered++;
            }

            for (int i = 0; i < baseStationPlaces.Length; i++) 
            {
                int singleBSType = 0;
                for (int j = 0; j < types.Length; j++)
                    singleBSType += ind.Gens[i * types.Length + j];
                if (singleBSType > 1)
                    return false;
            }
            ind.Cost = 0;
            for (int i = 0; i < baseStationPlaces.Length; i++)
            {
                for (int j = 0; j < types.Length; j++)
                    ind.Cost += ind.Gens[i * types.Length + j] * types[j].Cost;
            }

            return true;
        }

        private double Signal(BaseStationType type, double distance, int territoryType)
        {
            double d0 = 100;
            double cc = 3e8;
            double lambda = cc / type.fc;
            double a = 0, b = 0, c = 0;
            switch (territoryType)
            {
                case 0:
                    a = 4.6;
                    b = 0.0075;
                    c = 12.6;
                    break;
                case 1:
                    a = 4;
                    b = 0.0065;
                    c = 17.1;
                    break;
                case 2:
                    a = 3.6;
                    b = 0.005;
                    c = 20;
                    break;
            }

            double gamma = a - b * type.hb + c / ha;
            double Xf = 6 * Math.Log10(type.fc);
            double Xh = 20 * Math.Log10(type.hb / 2.0);
            double PL = 20 * Math.Log10(4 * Math.PI * 100 / lambda) + 10 * gamma * Math.Log10(distance / d0) + Xf + Xh;
            return type.W - PL;

        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private double DistanceInKmBetweenEarthCoordinates(double lat1, double lon1, double lat2, double lon2)
        {
            var earthRadiusKm = 6371;

            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            lat1 = DegreesToRadians(lat1);
            lat2 = DegreesToRadians(lat2);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadiusKm * c;
        }
    }
}
