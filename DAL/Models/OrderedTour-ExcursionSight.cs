using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class OrderedTour_ExcursionSight
    {
        [Key]
        public int OrderedTour_ExcursionSightId { get; set; }

        [ForeignKey("OrderedTour")]
        public int OrderedTourId { get; set; }

        [ForeignKey("ExcursionSight")]
        public int? ExcursionSightId { get; set; }

        public int OrdinalNumber { get; set; }

        public virtual OrderedTour OrderedTour { get; set; }
        public virtual ExcursionSight ExcursionSight { get; set; }

    }
}
