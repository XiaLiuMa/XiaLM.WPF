using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace XiaLM.P101.NetCoreDb.Entities
{
    /// <summary>
    /// 定义默认主键类型为Guid的实体基类
    /// </summary>
    public abstract class BaseEntity : BaseEntity<Guid>
    {

    }

    /// <summary>
    /// 泛型实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class BaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TPrimaryKey Id { get; set; }
    }
}
