using System;

namespace SeriousBusiness.Utils
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
