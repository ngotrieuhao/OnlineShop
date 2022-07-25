using FluentValidation;

namespace OnlineShop.ViewModels.Sales
{
    public class CouponUpdateRequestValidation : AbstractValidator<CouponUpdateRequest>

    {
        public CouponUpdateRequestValidation()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Coupon code cannot be empty")
                 .MaximumLength(6).WithMessage("Coupon code up to 6 characters");

            RuleFor(x => x.Count).NotEmpty().WithMessage("Coupon number cannot be empty")
                .LessThan(1000).WithMessage("The maximum number of coupons allowed to use is 1,000 times")
                .GreaterThan(0).WithMessage("The number of coupons allowed to use must be greater than 0");

            RuleFor(x => x.Promotion).NotEmpty().WithMessage("The percentage reduction cannot be empty")
                .LessThan(50).WithMessage("Up to 50% reduction percentage")
                .GreaterThan(0).WithMessage("The percentage reduction must be greater than 0");

            RuleFor(x => x.Describe).NotEmpty().WithMessage("Coupon description cannot be empty")
               .MaximumLength(4000).WithMessage("Coupon description up to 4000 characters");
        }
    }
}