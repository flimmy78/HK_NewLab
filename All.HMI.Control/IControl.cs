using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace All.Control.HMI
{
    public interface IControl
    {
        /// <summary>
        /// 边框粗细
        /// </summary>
        int BorderThickness { get; set; }

        /// <summary>
        /// 边框颜色
        /// </summary>
        Color BorderColor { get; set; }

        /// <summary>
        /// 是否有光晕
        /// </summary>
        bool Halo { get; set; }

        /// <summary>
        /// 控件方向
        /// </summary>
        System.Windows.Forms.Orientation Orientation { get; set; }
        /// <summary>
        /// 是否切割图形
        /// </summary>
        bool IsRegion { get; set; }
    }
}
