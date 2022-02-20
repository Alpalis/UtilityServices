#pragma warning disable IDE0060

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using OpenMod.API.Commands;
using OpenMod.API.Localization;

namespace OpenMod.Core.Commands
{
    public class CommandWrongActorException : UserFriendlyException
    {
        public CommandWrongActorException(string message) : base(message)
        {

        }

        public CommandWrongActorException(ICommandContext context, IStringLocalizer localizer) : base(localizer["commands:errors:wrong_actor"])
        {

        }

        public CommandWrongActorException(ICommandContext context) : this(context, context.ServiceProvider.GetRequiredService<IOpenModStringLocalizer>())
        {

        }
    }
}
