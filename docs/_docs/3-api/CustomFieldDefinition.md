---
title: CustomFieldDefinition
category: API
order: 40
---

Represents a custom field definition.

**Assembly:** Manatee.Trello.dll

**Namespace:** Manatee.Trello

**Inheritance hierarchy:**

- Object
- CustomFieldDefinition

## Properties

### [IBoard](../IBoard#iboard) Board { get; }

Gets the board on which the field is defined.

### string FieldGroup { get; }

Gets an identifier that groups fields across boards.

### string Id { get; }

Gets an ID on which matching can be performed.

### string Name { get; set; }

Gets or sets the name of the field.

### [IDropDownOptionCollection](../IDropDownOptionCollection#idropdownoptioncollection) Options { get; }

Gets drop down options, if applicable.

### [Position](../Position#position) Position { get; set; }

Gets or sets the position of the field.

### [CustomFieldType](../CustomFieldType#customfieldtype)? Type { get; }

Gets the data type of the field.

## Events

### Action&lt;[ICustomFieldDefinition](../ICustomFieldDefinition#icustomfielddefinition), IEnumerable&lt;string&gt;&gt; Updated

Raised when data on the custom field definition is updated.

## Methods

### Task Delete(CancellationToken ct = default(CancellationToken))

Deletes the field definition.

**Parameter:** ct

(Optional) A cancellation token for async processing.

### Task Refresh(CancellationToken ct = default(CancellationToken))

Refreshes the custom field definition data.

**Parameter:** ct

(Optional) A cancellation token for async processing.

### Task&lt;ICustomField&lt;double?&gt;&gt; SetValueForCard(ICard card, double? value, CancellationToken ct = default(CancellationToken))

Sets a value for a custom field on a card.

**Parameter:** card

The card on which to set the value.

**Parameter:** value

The vaue to set.

**Parameter:** ct

(Optional) A cancellation token for async processing.

**Returns:** The custom field instance.

### Task&lt;ICustomField&lt;bool?&gt;&gt; SetValueForCard(ICard card, bool? value, CancellationToken ct = default(CancellationToken))

Sets a value for a custom field on a card.

**Parameter:** card

The card on which to set the value.

**Parameter:** value

The vaue to set.

**Parameter:** ct

(Optional) A cancellation token for async processing.

**Returns:** The custom field instance.

### Task&lt;ICustomField&lt;string&gt;&gt; SetValueForCard(ICard card, string value, CancellationToken ct = default(CancellationToken))

Sets a value for a custom field on a card.

**Parameter:** card

The card on which to set the value.

**Parameter:** value

The vaue to set.

**Parameter:** ct

(Optional) A cancellation token for async processing.

**Returns:** The custom field instance.

### Task&lt;ICustomField&lt;IDropDownOption&gt;&gt; SetValueForCard(ICard card, IDropDownOption value, CancellationToken ct = default(CancellationToken))

Sets a value for a custom field on a card.

**Parameter:** card

The card on which to set the value.

**Parameter:** value

The vaue to set.

**Parameter:** ct

(Optional) A cancellation token for async processing.

**Returns:** The custom field instance.

### Task&lt;ICustomField&lt;DateTime?&gt;&gt; SetValueForCard(ICard card, DateTime? value, CancellationToken ct = default(CancellationToken))

Sets a value for a custom field on a card.

**Parameter:** card

The card on which to set the value.

**Parameter:** value

The vaue to set.

**Parameter:** ct

(Optional) A cancellation token for async processing.

**Returns:** The custom field instance.

### string ToString()

Returns a string that represents the current object.

**Returns:** A string that represents the current object.
