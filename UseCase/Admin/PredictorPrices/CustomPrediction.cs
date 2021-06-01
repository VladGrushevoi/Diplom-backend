using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Models;
using Services.AdminRepositories;



namespace UseCase.Admin.PredictorPrices
{
    public class CustomPrediction
    {
        IAdminRepository adminRepository;
        PrestigueDistrict prestigueDistrict;
        float _b;
        double[] _w;

        public CustomPrediction(IAdminRepository adminRepository, PrestigueDistrict prestigue)
        {
            this.adminRepository = adminRepository;
            this.prestigueDistrict = prestigue;
            this._b = 0;
            var data = FormXVectors(adminRepository);
            Fit(data.Item1, data.Item2);
        }

        public void Fit(double[,] X, double[,] y)
        {
            var input = ExtendInputWithOnes(X);
            var output = Matrix<double>.Build.DenseOfArray(y);

            var coeficients = ((input.Transpose() * input).Inverse() * input.Transpose() * output)
              		 	.Transpose().Row(0);
            _b = (float)coeficients.ElementAt(0);
            _w = SubArray(coeficients.ToArray(), 1, X.GetLength(1));
        }

        private Matrix<double> ExtendInputWithOnes(double[,] X)
        {
            var ones = Matrix<double>.Build.Dense(X.GetLength(0), 1, 1d);
            var extendedX = ones.Append(Matrix<double>.Build.DenseOfArray(X));

            return extendedX;
        }

        private double[] SubArray(double[] data, int index, int length)
        {
            double[] result = new double[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public double Predict(double[,] x)
        {
            var input = Matrix<double>.Build.DenseOfArray(x).Transpose();
            var w = Vector<double>.Build.DenseOfArray(_w);
            var price = input.Multiply(w).ToArray().Sum() + _b;
            var newPrice = price * prestigueDistrict.GetDistrictPrestigueValue((int)x[2,0]).Result;
            return newPrice;
        }

        private Tuple<double[,], double[,]> FormXVectors(IAdminRepository admin)
        {
            var apps = admin.GetAllApartment().Result.ToList();
            double[,] x = new double[apps.Count, 4];
            double[,] y = new double[apps.Count, 1];
            for(int i = 0; i < apps.Count; i++)
            {
                x[i,0] = apps[i].TotalSquare;
                x[i,1] = apps[i].RoomsCount;
                x[i,2] = apps[i].DistrictValue;
                x[i,3] = apps[i].Floor;
                y[i,0] = apps[i].Price;
            }
            return new Tuple<double[,], double[,]>(x,y);
        }
    }
}