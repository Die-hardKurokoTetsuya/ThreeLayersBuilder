/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       IConstructible.cs
 *  Desctiption:    可建造接口
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-15 08:47:53
 *
 *  Version:        V1.0.0
 ***********************************************/


using System.Text;

namespace Builder.Constructer
{
    /// <summary>
    /// 可建造接口
    /// </summary>
    public interface IConstructible
    {
        StringBuilder Construct();
    }
}
