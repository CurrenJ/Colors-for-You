using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    class ColorFade
    {
        private string hexCode;
        private int point;

        public ColorFade(string hex, int point) {
            hexCode = hex;
            this.point = point;
        }

        public Color getColor() {
            Color col;
            ColorUtility.TryParseHtmlString(hexCode, out col);
            return col;
        }

        public string getColorCode() {
            return hexCode;
        }

        public int getLocation() {
            return point;
        }
    }
}
