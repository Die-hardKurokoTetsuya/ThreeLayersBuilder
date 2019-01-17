/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       Constructer.cs
 *  Desctiption:    抽象建造者
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-01-15 08:38:29
 *
 *  Version:        V1.0.0
 ***********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder.Constructer
{
    public abstract class Constructer
    {
        private List<IConstructible> constructibles_List = new List<IConstructible>(20);

        /// <summary>
        /// 建造私有字段
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_PrivateField(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 GetModel
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_GetModel(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 Add
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_Add(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 Delete(string where)
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_Delete_Where(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 Delete(int id)
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_Delete_Id(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 Update
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_Update(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 Field
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_Field(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 GroupBy
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_GroupBy(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 OrderBy
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_OrderBy(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 Table
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_Table(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 Where
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_Where(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 Query
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_Query(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 GetList
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_GetList(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 DataTableToList
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_DataTableToList(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 GetPager
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_GetPager(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 ToString
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_ToString(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        public void Set_JointFullQueryString(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        /// <summary>
        /// 建造 Model
        /// </summary>
        /// <param name="constructible"></param>
        public void Set_Model(IConstructible constructible)
        {
            if (constructible == null)
                return;
            constructibles_List.Add(constructible);
        }

        public void Build()
        {
            foreach (IConstructible constructible in constructibles_List)
            {
                constructible.Construct();
            }   
        }
    }
}
