using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets
{
    class ColorManager
    {
        private List<ColorFade> colors;
        public int evenOddOffset;
        private Color colorRangeA;
        private Color colorRangeB;
        private bool colorRangeEnabled;

        private float minimumDisparityOver = 750;
        private float disparityOverNumOfCubes = 10;

        public ColorManager() {
            Debug.Log("Color Manager initialized.");
            colors = new List<ColorFade>();
            evenOddOffset = 0;
            randomNewColor(0);
            randomNewColor();
        }

        public ColorManager(Color from, Color to)
        {
            Debug.Log("Color Manager initialized with range.");
            colors = new List<ColorFade>();
            evenOddOffset = 0;

            colorRangeA = from;
            colorRangeB = to;
            colorRangeEnabled = true;

            randomNewColor(0);
            randomNewColor();
        }

        public void randomNewColor() {
            randomNewColor(colors[colors.Count - 1].getLocation() + UnityEngine.Random.Range(10, 20));
        }

        public void randomNewColor(int location)
        {
            Color background;
            int looped = 0;
            float highestDisp = 0;
            Color highestDispColor = Color.white;
            float currentDisp = 0;
            do
            {
                looped++;
                if (!colorRangeEnabled)
                {
                    background = new Color(
                        UnityEngine.Random.Range(0f, 1f),
                        UnityEngine.Random.Range(0f, 1f),
                        UnityEngine.Random.Range(0f, 1f)
                    );
                }
                else
                {
                    background = Color.Lerp(colorRangeA, colorRangeB, UnityEngine.Random.Range(0F, 1F));
                }

                if (colors.Count > 0) {
                    currentDisp = getColorDisparity(colors[colors.Count - 1].getColor(), background);
                    if (currentDisp > highestDisp)
                    {
                        Debug.Log(highestDisp +  " < " + currentDisp);
                        highestDisp = currentDisp;
                        highestDispColor = background;
                    }
                }

            } while (colors.Count > 0 && 
                    ((currentDisp
                            < minimumDisparityOver / ((location - colors[colors.Count-1].getLocation()) / disparityOverNumOfCubes) && looped < 25) 
                    || (currentDisp 
                            < minimumDisparityOver / ((location - colors[colors.Count - 1].getLocation()) / disparityOverNumOfCubes) / 2 && looped < 200)
                    || currentDisp 
                            < 30));
            if (colors.Count > 0)
            {
                Debug.Log(looped);
                Debug.Log("Color disparity: " + getColorDisparity(colors[colors.Count - 1].getColor(), highestDispColor) + " over " + (location - colors[colors.Count - 1].getLocation()) + " cubes.");
                Debug.Log("Color disparity bound 1: " + minimumDisparityOver / ((location - colors[colors.Count - 1].getLocation()) / disparityOverNumOfCubes));
                colors.Add(new ColorFade("#" + ColorUtility.ToHtmlStringRGB(highestDispColor), location));
            }
            else colors.Add(new ColorFade("#" + ColorUtility.ToHtmlStringRGB(background), location));
        }

        public void addColor(string hex, int location) {
            colors.Add(new ColorFade(hex, location));
        }

        public Color[] getStartAndEndColors(int y) {
            Vector2 bounds = new Vector2(0, 0);
            for (int j = 0; j < 2; j++) {
                for (int i = 0; i < colors.Count - 1; i++) {
                    bounds.x = colors[i].getLocation();
                    bounds.y = colors[i + 1].getLocation();
                    if (y >= bounds.x && y < bounds.y) {
                        Color[] colorBounds = { colors[i].getColor(), colors[i + 1].getColor() };
                        //Debug.Log("y: " + y + " | " + colors[i].getColorCode() + ", " + colors[i + 1].getColorCode());
                        return colorBounds;
                    }
                }
                randomNewColor();
                if (colors.Count > 10)
                {
                    colors.RemoveAt(0);
                    adjustOffset();
                }
            }
            Color[] defaultColors = {Color.white, Color.white};
            return defaultColors;
        }

        public bool isEvenColorFade(int y) {
            Vector2 bounds = new Vector2(0, 0);
            for (int j = 0; j < 2; j++) {
                for (int i = 0; i < colors.Count - 1; i++)
                {
                    bounds.x = colors[i].getLocation();
                    bounds.y = colors[i + 1].getLocation();
                    if (y >= bounds.x && y < bounds.y)
                    {
                        Color[] colorBounds = { colors[i].getColor(), colors[i + 1].getColor() };
                        return (((i+evenOddOffset) % 2 + "").Equals("0"));
                    }
                }
                randomNewColor();
                if (colors.Count > 10)
                {
                    colors.RemoveAt(0);
                    adjustOffset();
                }
            }
            return false;
        }

        public int getColorBoundsStart(int y) {
            Vector2 bounds = new Vector2(0, 0);
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < colors.Count - 1; i++)
                {
                    bounds.x = colors[i].getLocation();
                    bounds.y = colors[i + 1].getLocation();
                    if (y >= bounds.x && y < bounds.y)
                    {
                        return (int)bounds.x;
                    }
                }
                randomNewColor();
                if (colors.Count > 10)
                {
                    colors.RemoveAt(0);
                    adjustOffset();
                }
            }
            return -1;
        }

        public int getColorBoundsLength(int y)
        {
            Vector2 bounds = new Vector2(0, 0);
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < colors.Count - 1; i++)
                {
                    bounds.x = colors[i].getLocation();
                    bounds.y = colors[i + 1].getLocation();
                    if (y >= bounds.x && y < bounds.y)
                    {
                        return (int)bounds.y - (int)bounds.x;
                    }
                }
                randomNewColor();
                if (colors.Count > 10)
                {
                    colors.RemoveAt(0);
                    adjustOffset();
                }
            }
            return -1;
        }

        public int getEndColorBound(int y) {
            Vector2 bounds = new Vector2(0, 0);
            for (int i = 0; i < colors.Count - 1; i++)
            {
                bounds.x = colors[i].getLocation();
                bounds.y = colors[i + 1].getLocation();
                if (y >= bounds.x && y < bounds.y)
                {
                    return (int)bounds.y;
                }
            }
            return -1;
        }

        private void adjustOffset() {
            if (evenOddOffset == 0)
                evenOddOffset = 1;
            if (evenOddOffset == 1)
                evenOddOffset = 0;
        }

        private float getColorDisparity(Color a, Color b) {
            float aR = a.r * 255F;
            float bR = b.r * 255F;
            float aG = a.g * 255F;
            float bG = b.g * 255F;
            float aB = a.b * 255F;
            float bB = b.b * 255F;
            float r = (aR + bR) / 2F;
            float deltaC = Mathf.Sqrt((2 + r / 256F) * Mathf.Pow(bR-aR, 2) + 4 * Mathf.Pow(bG-aG, 2) + (2 + (255 - r) / 256F) * Mathf.Pow(bB-aB, 2));
            return deltaC;
        }
    }
}
