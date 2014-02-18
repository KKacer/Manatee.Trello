﻿/***************************************************************************************

	Copyright 2013 Greg Dennis

	   Licensed under the Apache License, Version 2.0 (the "License");
	   you may not use this file except in compliance with the License.
	   You may obtain a copy of the License at

		 http://www.apache.org/licenses/LICENSE-2.0

	   Unless required by applicable law or agreed to in writing, software
	   distributed under the License is distributed on an "AS IS" BASIS,
	   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	   See the License for the specific language governing permissions and
	   limitations under the License.
 
	File Name:		CachingEntityRepository.cs
	Namespace:		Manatee.Trello.Internal.DataAccess
	Class Name:		CachingEntityRepository
	Purpose:		Decorates an implementation of IEntityRepository with caching.

***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello.Contracts;

namespace Manatee.Trello.Internal.DataAccess
{
	internal class CachingEntityRepository : IEntityRepository
	{
		private readonly IEntityRepository _innerRepository;
		private readonly ICache _cache;

		public TimeSpan EntityDuration { get { return _innerRepository.EntityDuration; } }
		public bool AllowSelfUpdate { get; set; }

		public CachingEntityRepository(IEntityRepository innerRepository, ICache cache)
		{
			_innerRepository = innerRepository;
			_cache = cache;
		}

		public bool Refresh<T>(T entity, EntityRequestType request)
			where T : ExpiringObject
		{
			return _innerRepository.Refresh(entity, request);
		}
		public bool RefreshCollection<T>(ExpiringObject list, EntityRequestType request)
			where T : ExpiringObject, IEquatable<T>, IComparable<T>
		{
			var enumerable = list as ExpiringList<T>;
			var retVal = _innerRepository.RefreshCollection<T>(list, request);
			foreach (var entity in enumerable)
			{
				_cache.Add(entity);
			}
			return retVal;
		}
		public T Download<T>(EntityRequestType request, IDictionary<string, object> parameters)
			where T : ExpiringObject
		{
			T entity = null;
			try
			{
				var id = parameters.SingleOrDefault(kvp => kvp.Key.In("_id")).Value;
				Func<T> query = () => DownloadAndAddToCache<T>(request, parameters);
				entity = id == null ? query() : _cache.Find(e => e.Matches(id.ToString()), query);
				entity.EntityRepository = this;
				entity.PropagateDependencies();
				return entity;
			}
			catch (Exception)
			{
				_cache.Remove(entity);
				throw;
			}
		}
		public IEnumerable<T> GenerateList<T>(ExpiringObject owner, EntityRequestType request, string filter)
			where T : ExpiringObject, IEquatable<T>, IComparable<T>
		{
			var list = _innerRepository.GenerateList<T>(owner, request, filter);
			return _cache.Find(l => l.Equals(list), () => list);
		}
		public void Upload(EntityRequestType request, IDictionary<string, object> parameters)
		{
			_innerRepository.Upload(request, parameters);
		}
		public void NetworkStatusChanged(object sender, EventArgs e) {}

		private T DownloadAndAddToCache<T>(EntityRequestType request, IDictionary<string, object> parameters)
			where T : ExpiringObject
		{
			var entity = _innerRepository.Download<T>(request, parameters);
			_cache.Add(entity);
			return entity;
		}
	}
}