//using Nop.Core.Domain.Logging;
//using Nop.Services.Tasks;
//using System;
//using Nop.Services.Logging;
//using Nop.Core.Domain.Orders;
//using System.Linq;
//using Nop.Services.Orders;
//using Nop.Services.Shipping;
//using System.Collections.Generic;
//using Nop.Data;

//namespace Nop.Plugin.NopFeatures.ShipRocket.ScheduleTasks
//{
//    public partial class Tracking : IScheduleTask
//    {
//        #region #region Fields

//        private readonly ILogger _logger;
//        private readonly IOrderService _orderService;
//        private readonly IShipmentService _shipmentService;
//        private readonly IRepository<OrderNote> _orderNoteRepository;

//        #endregion

//        #region Constructors

//        public Tracking(ILogger logger,
//                        IOrderService orderService,
//                        IShipmentService shipmentService,
//                        IRepository<OrderNote> orderNoteRepository)
//        {
//            this._logger = logger;
//            this._orderService = orderService;
//            this._shipmentService = shipmentService;
//            this._orderNoteRepository = orderNoteRepository;
//        }        

//        #endregion

//        public void Execute()
//        {
//            try
//            {
//                //new orders list
//                IList<OrderShipment> newOrders = new List<OrderShipment>();

//                //all orders
//                var allOrders = _orderService.SearchOrders();
//                foreach (var aOrders in allOrders)
//                {
//                    //shipment
//                    var ordersItemShipment = _shipmentService.GetShipmentsByOrderId(aOrders.Id);                    
//                    if(ordersItemShipment.Count() == 0)
//                    {
//                        var query = (from a in _orderNoteRepository.Table
//                                     where a.OrderId.Equals(aOrders.Id)
//                                     select a);

//                        foreach (var orderNotesItem in query)
//                        {
//                            if (orderNotesItem.Note.Contains("Ship Rocket Order Id"))
//                            {
//                                string[] notes = orderNotesItem.Note.Split('-');
//                                var orderShipment = new OrderShipment()
//                                {
//                                    Order = aOrders,
//                                    ShipRocketOrderId = Convert.ToInt32(notes[2])
//                                };
//                                newOrders.Add(orderShipment);
//                            }
//                        }
//                    }                    
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.InsertLog(LogLevel.Error, "Tracking schedule task error " + ex.InnerException, ex.Message + "stacktrace" + ex.StackTrace);
//            }
//        }

//        public class OrderShipment
//        {
//            public virtual Order Order { get; set; }
//            public int ShipRocketOrderId { get; set; }
//        }
//    }
//}
