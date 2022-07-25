﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.Catalog.Categories
{
    public class CategoryCreateRequestValidation : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateRequestValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name cannot be empty")
                .MaximumLength(200).WithMessage("Category name cannot exceed 200 characters");
        }
    }
}
