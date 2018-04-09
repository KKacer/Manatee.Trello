﻿using System.Threading;
using System.Threading.Tasks;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of <see cref="ISticker"/>s.
	/// </summary>
	public interface ICardStickerCollection
	{
		/// <summary>
		/// Adds a <see cref="ISticker"/> to a <see cref="Card"/>.
		/// </summary>
		/// <param name="name">The name of the sticker.</param>
		/// <param name="left">The position of the left edge.</param>
		/// <param name="top">The position of the top edge.</param>
		/// <param name="zIndex">The z-index. Default is 0.</param>
		/// <param name="rotation">The rotation. Default is 0.</param>
		/// <returns>The attachment generated by Trello.</returns>
		/// <exception cref="ValidationException{T}">Thrown when <paramref name="name"/> is null, empty, or whitespace.</exception>
		/// <exception cref="ValidationException{Int32}">Thrown when <paramref name="rotation"/> is less than 0 or greater than 359.</exception>
		Task<ISticker> Add(string name, double left, double top, int zIndex = 0, int rotation = 0,
		                   CancellationToken ct = default(CancellationToken));
	}
}