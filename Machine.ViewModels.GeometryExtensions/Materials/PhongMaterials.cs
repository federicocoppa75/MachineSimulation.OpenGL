using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Materials
{
    public static class PhongMaterials
    {
        static public IList<Material> Materials { get; private set; }

        static PhongMaterials()
        {
            Materials = new List<Material>
            {
                Black,
                BlackPlastic,
                BlackRubber,
                Blue,
                Brass,
                Bronze,
                Chrome,
                Copper,
                DefaultVRML,
                Emerald,
                Glass,
                Gold,
                Green,
                Indigo,
                Jade,
                LightGray,
                MediumGray,
                Obsidian,
                Orange,
                Pearl,
                Pewter,
                PolishedBronze,
                PolishedCopper,
                PolishedGold,
                Red,
                Silver,
                Turquoise,
                Violet,
                White,
                Yellow
            };
        }

        public static Vector4 ToColor(float r, float g, float b, float a) => new Vector4(r, g, b, a);
        public static Vector4 ToColor(double r, double g, double b, double a) => ToColor((float)r, (float)g, (float)b, (float)a);

        public static Material Red => new Material()
        {
            Name = "Red",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = Color.Red,
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Blue => new Material()
        {
            Name = "Blue",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = Color.Blue,
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Green => new Material()
        {
            Name = "Green",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = Color.Green,
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Orange => new Material()
        {
            Name = "Orange",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = ToColor(0.992157, 0.513726, 0.0, 1.0),
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material BlanchedAlmond => new Material()
        {
            Name = "BlanchedAlmond",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = Color.BlanchedAlmond,
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Bisque => new Material()
        {
            Name = "Bisque",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = Color.Bisque,
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Yellow => new Material()
        {
            Name = "Yellow",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = ToColor(1.0, 0.964706, 0.0, 1.0),
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Indigo => new Material()
        {
            Name = "Indigo",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = ToColor(0.0980392, 0.0, 0.458824, 1.0),
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Violet => new Material()
        {
            Name = "Violet",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = ToColor(0.635294, 0.0, 1.0, 1.0),
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material White => new Material()
        {
            Name = "White",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = ToColor(0.992157, 0.992157, 0.992157, 1.0),
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material PureWhite => new Material()
        {
            Name = "PureWhite",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = Color.White,
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Black => new Material()
        {
            Name = "Black",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = Color.Black,
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Gray => new Material()
        {
            Name = "Gray",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = ToColor(0.254902, 0.254902, 0.254902, 1.0),
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material MediumGray => new Material()
        {
            Name = "MediumGray",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = ToColor(0.454902, 0.454902, 0.454902, 1.0),
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material LightGray => new Material()
        {
            Name = "LightGray",
            Ambient = ToColor(0.1, 0.1, 0.1, 1.0),
            Diffuse = ToColor(0.682353, 0.682353, 0.682353, 1.0),
            Specular = ToColor(0.0225, 0.0225, 0.0225, 1.0),
            Shininess = 12.8f
        };

        public static Material Glass => new Material()
        {
            Name = "Glass",
            Ambient = ToColor(0.0, 0.0, 0.0, 1.0),
            Diffuse = ToColor(0.588235, 0.670588, 0.729412, 1.0),
            Specular = ToColor(0.9, 0.9, 0.9, 1.0),
            Shininess = 96.0f
        };

        public static Material Brass => new Material()
        {
            Name = "Brass",
            Ambient = ToColor(0.329412, 0.223529, 0.027451, 1.0),
            Diffuse = ToColor(0.780392, 0.568627, 0.113725, 1.0),
            Specular = ToColor(0.992157, 0.941176, 0.807843, 1.0),
            Shininess = 27.8974f
        };

        public static Material Bronze => new Material()
        {
            Name = "Bronze",
            Ambient = ToColor(0.2125, 0.1275, 0.054, 1.0),
            Diffuse = ToColor(0.714, 0.4284, 0.18144, 1.0),
            Specular = ToColor(0.393548, 0.271906, 0.166721, 1.0),
            Shininess = 25.6f
        };

        public static Material PolishedBronze => new Material()
        {
            Name = "PolishedBronze",
            Ambient = ToColor(0.25, 0.148, 0.06475, 1.0),
            Diffuse = ToColor(0.4, 0.2368, 0.1036, 1.0),
            Specular = ToColor(0.774597, 0.458561, 0.200621, 1.0),
            Shininess = 76.8f
        };

        public static Material Chrome => new Material()
        {
            Name = "Chrome",
            Ambient = ToColor(0.25f, 0.25f, 0.25f, 1.0f),
            Diffuse = ToColor(0.4f, 0.4f, 0.4f, 1.0f),
            Specular = ToColor(0.774597f, 0.774597f, 0.774597f, 1.0f),
            Shininess = 76.8f
        };

        public static Material Copper => new Material()
        {
            Name = "Copper",
            Ambient = ToColor(0.19125, 0.0735, 0.0225, 1.0),
            Diffuse = ToColor(0.7038, 0.27048, 0.0828, 1.0),
            Specular = ToColor(0.256777, 0.137622, 0.086014, 1.0),
            Shininess = 12.8f
        };

        public static Material PolishedCopper => new Material()
        {
            Name = "PolishedCopper",
            Ambient = ToColor(0.2295, 0.08825, 0.0275, 1.0),
            Diffuse = ToColor(0.5508, 0.2118, 0.066, 1.0),
            Specular = ToColor(0.580594, 0.223257, 0.0695701, 1.0),
            Shininess = 51.2f
        };

        public static Material Gold => new Material()
        {
            Name = "Gold",
            Ambient = ToColor(0.24725, 0.1995, 0.0745, 1.0),
            Diffuse = ToColor(0.75164, 0.60648, 0.22648, 1.0),
            Specular = ToColor(0.628281, 0.555802, 0.366065, 1.0),
            Shininess = 51.2f
        };

        public static Material PolishedGold => new Material()
        {
            Name = "PolishedGold",
            Ambient = ToColor(0.24725, 0.2245, 0.0645, 1.0),
            Diffuse = ToColor(0.34615, 0.3143, 0.0903, 1.0),
            Specular = ToColor(0.797357, 0.723991, 0.208006, 1.0),
            Shininess = 83.2f
        };

        public static Material Pewter => new Material()
        {
            Name = "Pewter",
            Ambient = ToColor(0.105882, 0.058824, 0.113725, 1.0),
            Diffuse = ToColor(0.427451, 0.470588, 0.541176, 1.0),
            Specular = ToColor(0.333333, 0.333333, 0.521569, 1.0),
            Shininess = 9.84615f
        };

        public static Material Silver => new Material()
        {
            Name = "Silver",
            Ambient = ToColor(0.19225, 0.19225, 0.19225, 1.0),
            Diffuse = ToColor(0.50754, 0.50754, 0.50754, 1.0),
            Specular = ToColor(0.508273, 0.508273, 0.508273, 1.0),
            Shininess = 51.2f
        };

        public static Material Emerald => new Material()
        {
            Name = "Emerald",
            Ambient = ToColor(0.0215, 0.1745, 0.0215, 0.55),
            Diffuse = ToColor(0.07568, 0.61424, 0.07568, 0.55),
            Specular = ToColor(0.633, 0.727811, 0.633, 0.55),
            Shininess = 76.8f
        };

        public static Material Jade => new Material()
        {
            Name = "Jade",
            Ambient = ToColor(0.135, 0.2225, 0.1575, 0.95),
            Diffuse = ToColor(0.54, 0.89, 0.63, 0.95),
            Specular = ToColor(0.316228, 0.316228, 0.316228, 0.95),
            Shininess = 12.8f
        };

        public static Material Obsidian => new Material()
        {
            Name = "Obsidian",
            Ambient = ToColor(0.05375, 0.05, 0.06625, 0.82),
            Diffuse = ToColor(0.18275, 0.17, 0.22525, 0.82),
            Specular = ToColor(0.332741, 0.328634, 0.346435, 0.82),
            Shininess = 38.4f
        };

        public static Material Pearl => new Material()
        {
            Name = "Pearl",
            Ambient = ToColor(0.25, 0.20725, 0.20725, 0.922),
            Diffuse = ToColor(1.0, 0.829, 0.829, 0.922),
            Specular = ToColor(0.296648, 0.296648, 0.296648, 0.922),
            Shininess = 11.264f
        };

        public static Material Turquoise => new Material()
        {
            Name = "Turquoise",
            Ambient = ToColor(0.1, 0.18725, 0.1745, 0.8),
            Diffuse = ToColor(0.396, 0.74151, 0.69102, 0.8),
            Specular = ToColor(0.297254, 0.30829, 0.306678, 0.8),
            Shininess = 12.8f
        };

        public static Material BlackPlastic => new Material()
        {
            Name = "BlackPlastic",
            Ambient = ToColor(0.0, 0.0, 0.0, 1.0),
            Diffuse = ToColor(0.01, 0.01, 0.01, 1.0),
            Specular = ToColor(0.50, 0.50, 0.50, 1.0),
            Shininess = 32f
        };

        public static Material BlackRubber => new Material()
        {
            Name = "BlackRubber",
            Ambient = ToColor(0.02, 0.02, 0.02, 1.0),
            Diffuse = ToColor(0.01, 0.01, 0.01, 1.0),
            Specular = ToColor(0.4, 0.4, 0.4, 1.0),
            Shininess = 10f
        };

        public static Material DefaultVRML => new Material()
        {
            Name = "DefaultVRML",
            Ambient = ToColor(0.2, 0.2, 0.2, 1.0),
            Diffuse = ToColor(0.8, 0.8, 0.8, 1.0),
            Specular = ToColor(0.0, 0.0, 0.0, 1.0),
            Shininess = 25.6f
        };

    }
}
