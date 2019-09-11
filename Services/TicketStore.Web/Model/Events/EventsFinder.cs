using System;
using System.Collections.Generic;
using System.Linq;
using TicketStore.Data;
using TicketStore.Data.Model;

namespace TicketStore.Web.Model.Events
{
    public class EventsFinder
    {
        private readonly ApplicationContext _db;
        private readonly Int32 _merchantId;
        private readonly IDateTimeProvider _dateTime;

        public EventsFinder(ApplicationContext context, Int32 merchantId, IDateTimeProvider dateTime)
        {
            _db = context;
            _merchantId = merchantId;
            _dateTime = dateTime;
        }
        
        public List<Event> Find()
        {
            return _db.Events
                .Where(e =>
                    e.MerchantId == _merchantId
                    && e.Time - _dateTime.Now >= TimeSpan.FromHours(3) 
                )
                .ToList();
        }
    }
}
