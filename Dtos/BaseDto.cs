namespace PizzaOrder.Dtos

{
    public class BaseDto
    {
        public string CreatedDateTime { get; set; } = "";
        //public int CreatedById { get; set; } = 0;
        public string CreatedByName { get; set; } = "";
        public string UpdatedDateTime { get; set; } = "";
        //public int UpdatedById { get; set; } = 0;
        public string UpdatedByName { get; set; } = "";
        public bool? Active { get; set; } = null;
    }

}
