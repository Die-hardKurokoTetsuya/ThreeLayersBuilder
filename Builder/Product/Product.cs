/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       Product.cs
 *  Desctiption:    抽象产品类
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-15 08:41:05
 *
 *  Version:        V1.0.0
 ***********************************************/

using System.Text;

namespace Builder.Product
{
    public class Product
    {
        private readonly StringBuilder contents;

        public Product(StringBuilder contents)
        {
            this.contents = contents;
        }

        /// <summary>
        /// 导出为文件
        /// </summary>
        /// <param name="path">路径</param>
        public void Export(string path)
        {

        }
    }
}
