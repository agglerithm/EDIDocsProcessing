using System;

namespace EdiMessages
{
    [Serializable]
    public class Package
    {
        public int IndexNumber
        {
            get; set;
        }
        public PackageSpecification Specifications
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string CustomerReference
        {
            get; set;
        }

        public override string ToString()
        {
            return string.Format("{4}[{5}]{4} IndexNumber: {0}{4} Specifications: {1}{4} Description: {2}{4} CustomerReference: {3}{4}",
                IndexNumber, Specifications, Description, CustomerReference, Environment.NewLine, GetType().Name);
        }
    }


    public class PackageSpecification
    {
        public int Length
        {
            get;
            set;
        }

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public int Weight
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{4}[{5}]{4} Length: {0}{4} Width: {1}{4} Height: {2}{4} Weight: {3}{4}", 
                Length, Width, Height, Weight, Environment.NewLine, GetType().Name);
        }
    }
}