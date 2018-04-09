﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Internal.Validation;
using Manatee.Trello.Json;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of <see cref="ISticker"/>s.
	/// </summary>
	public class CardStickerCollection : ReadOnlyStickerCollection, ICardStickerCollection
	{
		private static readonly NumericRule<int> RotationRule = new NumericRule<int>{Min = 0, Max = 359};

		internal CardStickerCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth) {}

		/// <summary>
		/// Adds a <see cref="ISticker"/> to a <see cref="Card"/>.
		/// </summary>
		/// <param name="name">The name of the sticker.</param>
		/// <param name="left">The position of the left edge.</param>
		/// <param name="top">The position of the top edge.</param>
		/// <param name="zIndex">The z-index. Default is 0.</param>
		/// <param name="rotation">The rotation. Default is 0.</param>
		/// <returns>The attachment generated by Trello.</returns>
		/// <exception cref="ValidationException{String}">Thrown when <paramref name="name"/> is null, empty, or whitespace.</exception>
		/// <exception cref="ValidationException{Int32}">Thrown when <paramref name="rotation"/> is less than 0 or greater than 359.</exception>
		public async Task<ISticker> Add(string name, double left, double top, int zIndex = 0, int rotation = 0,
		                                CancellationToken ct = default(CancellationToken))
		{
			var error = NotNullOrWhiteSpaceRule.Instance.Validate(null, name);
			if (error != null)
				throw new ValidationException<string>(name, new[] {error});
			error = RotationRule.Validate(null, rotation);
			if (error != null)
				throw new ValidationException<int>(rotation, new[] {error});

			var parameters = new Dictionary<string, object>
				{
					{"image", name},
					{"top", top},
					{"left", left},
					{"zIndex", zIndex},
					{"rotate", rotation},
				};
			var endpoint = EndpointFactory.Build(EntityRequestType.Card_Write_AddSticker, new Dictionary<string, object> {{"_id", OwnerId}});
			var newData = await JsonRepository.Execute<IJsonSticker>(Auth, endpoint, ct, parameters);

			return new Sticker(newData, OwnerId, Auth);
		}

		/// <summary>
		/// Removes a sticker from a card.
		/// </summary>
		/// <param name="sticker">The sticker to remove.</param>
		public async Task Remove(Sticker sticker, CancellationToken ct = default(CancellationToken))
		{
			var error = NotNullRule<Sticker>.Instance.Validate(null, sticker);
			if (error != null)
				throw new ValidationException<Sticker>(sticker, new[] {error});

			var endpoint = EndpointFactory.Build(EntityRequestType.Card_Write_RemoveSticker,
			                                     new Dictionary<string, object>
				                                     {
					                                     {"_id", OwnerId},
					                                     {"_stickerId", sticker.Id}
				                                     });
			await JsonRepository.Execute(Auth, endpoint, ct);
		}
	}
}