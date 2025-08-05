using System;

namespace Dealio.Services.Helpers
{
    public enum ServiceResultEnum
    {
        Success = 1,
        Created,
        Updated,
        Deleted,
        Failed,
        NotFound,
        Already_Exist,
        Ordered,
        Empty,
        SameSellerAndBuyer,
        AlreadyAssigned,
        UserAlreadyExists,
        NotConfirmed,
        LockedOut,
        IncorrectEmailOrPassword
    }
}
