using System;
using System.Linq;
using Services.AdminRepositories;

namespace UseCase.Admin.PredictorPrices
{
    public class KnnClasification
    {
        IAdminRepository adminRepository;
        PrestigueDistrict prestigue;

        public KnnClasification(IAdminRepository adminRepository, PrestigueDistrict prestigue)
        {
            this.adminRepository = adminRepository;
            this.prestigue = prestigue;
        }

        public float PredictPrice()
        {
            double[][] trainData = LoadData();
            //int numFeatures = 4;
            int numClasses = 3;
            double[] unknown = new double[] { 5.25, 1.75 };
            int k = 1;
            int predicted = Classify(unknown, trainData,
              numClasses, k);
            // k = 4;
            // predicted = Classify(unknown, trainData,
            //   numClasses, k);
            Console.WriteLine("Predicted class = " + predicted);

            return 0;
        }
        int Classify(double[] unknown,
         double[][] trainData, int numClasses, int k)
        {
            int n = trainData.Length;
            IndexAndDistance[] info = new IndexAndDistance[n];
            for (int i = 0; i < n; ++i)
            {
                IndexAndDistance curr = new IndexAndDistance();
                double dist = Distance(unknown, trainData[i]);
                curr.idx = i;
                curr.dist = dist;
                info[i] = curr;
            }
            Array.Sort(info);  // sort by distance
            for (int i = 0; i < k; ++i)
            {
                int c = (int)trainData[info[i].idx][2];
                string dist = info[i].dist.ToString("F3");
                Console.WriteLine("( " + trainData[info[i].idx][0] +
                  "," + trainData[info[i].idx][1] + " )  :  " +
                  dist + "        " + c);
            }
            int result = Vote(info, trainData, numClasses, k);
            return result;
        }
        int Vote(IndexAndDistance[] info,
          double[][] trainData, int numClasses, int k)
        {
            int[] votes = new int[numClasses];  // One cell per class
            for (int i = 0; i < k; ++i)
            {       // Just first k
                int idx = info[i].idx;            // Which train item
                int c = (int)trainData[idx][2];   // Class in last cell
                ++votes[c];
            }
            int mostVotes = 0;
            int classWithMostVotes = 0;
            for (int j = 0; j < numClasses; ++j)
            {
                if (votes[j] > mostVotes)
                {
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }
            }
            return classWithMostVotes;
        }
        double Distance(double[] unknown,
          double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < unknown.Length; ++i)
                sum += (unknown[i] - data[i]) * (unknown[i] - data[i]);
            return Math.Sqrt(sum);
        }
         double[][] LoadData()
        {
            var apps = adminRepository.GetAllApartment().Result.ToList();
            double[][] x = new double[apps.Count][];
            for(int i = 0; i < apps.Count; i++)
            {
                x[i] = new double[5]{ apps[i].TotalSquare, apps[i].RoomsCount, apps[i].DistrictValue, apps[i].Floor, apps[i].Price };
            }
            return x;
        }

    }

    public class IndexAndDistance : IComparable<IndexAndDistance>
    {
        public int idx;
        public double dist;
        public int CompareTo(IndexAndDistance other)
        {
            if (this.dist < other.dist) return -1;
            else if (this.dist > other.dist) return +1;
            else return 0;
        }
    }
}