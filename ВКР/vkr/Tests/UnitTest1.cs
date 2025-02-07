using GMap.NET;
using vkr;

namespace TestProject1
{
    public class MethodTests
    {
        [Fact]
        public void PreparationTest()
        {
            PointLatLng[][] points =
            [
                [new PointLatLng(0, 0), new PointLatLng(1, 0)]
            ];
            PointLatLng[] baseStaions = [new PointLatLng(0, 0)];
            BaseStationType[] types = [new BaseStationType() { Cost = 1, fc = 150, hb = 10, W = 20 }];
            Method method = new Method(points, baseStaions, types, 1);

            method.Preparation(0);

            int[][] coverage = method.Coverage;
            int[][] expected =
                [
                    [1, 0]
                ];
            Assert.Equal(1, coverage.Length);
            for (int i = 0; i < coverage.Length; i++)
            {
                for (int j = 0; j < coverage[i].Length; j++)
                {
                    Assert.Equal(expected[i][j], coverage[i][j]);
                }
            }
        }
        [Fact]
        public void CheckIndividualTest()
        {
            PointLatLng[][] points =
            [
                [new PointLatLng(0, 0), new PointLatLng(1, 0)]
            ];
            PointLatLng[] baseStaions = [new PointLatLng(0, 0)];
            BaseStationType[] types = [new BaseStationType() { Cost = 1, fc = 150, hb = 10, W = 20 }];
            var method = new Method(points, baseStaions, types, 1);
            method.Preparation(0);
            var individual = new Individual() { Gens = [1] };
            
            var res = method.CheckIndividual(individual);

            Assert.True(res);
            Assert.Equal(1, individual.Cost);
            Assert.Equal(1, individual.covered);
        }
    }
}