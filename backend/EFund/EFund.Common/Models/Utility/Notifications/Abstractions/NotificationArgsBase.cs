using System.Text.Json.Serialization;

namespace EFund.Common.Models.Utility.Notifications.Abstractions;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type"),
 JsonDerivedType(typeof(ComplaintResponseForRequestedByArgs), "ComplaintResponse"),
 JsonDerivedType(typeof(ComplaintRequestChangesForRequestedForArgs), "ComplaintRequestChanges"),
 JsonDerivedType(typeof(ComplaintAcceptedForRequestedForArgs), "ComplaintAccepted")]
public abstract class NotificationArgsBase;