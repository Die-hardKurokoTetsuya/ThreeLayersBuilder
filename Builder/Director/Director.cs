/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       Director.cs
 *  Desctiption:    抽象指挥者
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-15 08:37:09
 *
 *  Version:        V1.0.0
 ***********************************************/


namespace Builder.Director
{
    public abstract class Director
    {
        private readonly Constructer.Constructer constructer;

        /// <summary>
        /// 初始化指挥者
        /// </summary>
        /// <param name="constructer">具体建造者</param>
        public Director(Constructer.Constructer constructer)
        {
            this.constructer = constructer;
        }

        public void Build()
        {
            constructer.Build();
        }
    }
}
