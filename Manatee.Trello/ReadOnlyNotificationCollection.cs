﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manatee.Trello.Internal;
using Manatee.Trello.Internal.Caching;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Json;

namespace Manatee.Trello
{
	/// <summary>
	/// A read-only collection of actions.
	/// </summary>
	public class ReadOnlyNotificationCollection : ReadOnlyCollection<INotification>, IReadOnlyNotificationCollection
	{
		internal ReadOnlyNotificationCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth) {}

		/// <summary>
		/// Adds a filter to the collection.
		/// </summary>
		/// <param name="filter">The filter type.</param>
		public void Filter(NotificationType filter)
		{
			var filters = filter.GetFlags().Cast<NotificationType>();
			Filter(filters);
		}

		/// <summary>
		/// Adds a set of filters to the collection.
		/// </summary>
		/// <param name="filters">A collection of filters.</param>
		public void Filter(IEnumerable<NotificationType> filters)
		{
			var filter = (string)AdditionalParameters["filter"];
			if (!filter.IsNullOrWhiteSpace())
				filter += ",";
			filter += filters.Select(a => a.GetDescription()).Join(",");
			AdditionalParameters["filter"] = filter;
		}

		/// <summary>
		/// Manually updates the collection's data.
		/// </summary>
		/// <param name="force">Indicates that the refresh should ignore the value in <see cref="TrelloConfiguration.RefreshThrottle"/> and make the call to the API.</param>
		/// <param name="ct">(Optional) A cancellation token for async processing.</param>
		public sealed override async Task Refresh(bool force = false, CancellationToken ct = default(CancellationToken))
		{
			IncorporateLimit();

			var endpoint = EndpointFactory.Build(EntityRequestType.Member_Read_Notifications, new Dictionary<string, object> {{"_id", OwnerId}});
			var newData = await JsonRepository.Execute<List<IJsonNotification>>(Auth, endpoint, ct, AdditionalParameters);

			Items.Clear();
			Items.AddRange(newData.Select(jn =>
				{
					var notification = jn.GetFromCache<Notification, IJsonNotification>(Auth);
					notification.Json = jn;
					return notification;
				}));
		}
	}
}