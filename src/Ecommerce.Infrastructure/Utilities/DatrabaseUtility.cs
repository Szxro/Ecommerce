using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Utilities;

public static class DatabaseUtility
{
    public static Task SeedAsync(DbContext context,bool stored,CancellationToken cancellationToken)
    {
        //Template template = new Template
        //{
        //    Title = "Email Verification Code",
        //    Content = @"Hello, {{ FirstName }} {{ LastName }},

        //            We received a request to verify your email address ({{ Email }}).  
        //            Use the following verification code to complete the process:

        //            {{ EmailCode }}

        //            If you did not request this, you can ignore this email.

        //            Thank you,  
        //            The Team",
        //    IsDefault = true,
        //    TemplateCategory = new TemplateCategory
        //    {
        //        Name = "email",
        //        Description = "Template use for email notification."
        //    }
        //};

        //context.Set<Template>().Add(template);

        //await context.SaveChangesAsync(cancellationToken);\

        return Task.CompletedTask;
    }
}
