<div style="font-family: 'Segoe UI', 'Inter', 'Helvetica Neue', system-ui, sans-serif; line-height: 1.6;">

# Arrays and Lists in C\#

> **Article** &middot; Applies to: **.NET 10** &middot; **C# 14** &middot; Skill level: Beginner → Intermediate

Arrays (`T[]`) and lists (`List<T>`) are the two most widely used sequential
collection types in the .NET Base Class Library. They look similar on the
surface — both store items in order and both allow indexed access — but they
make very different trade-offs between **memory layout**, **flexibility**, and
**performance**.

This article explains how each type is laid out in memory, when to choose one
over the other, and how to use them idiomatically in modern C# 14 on .NET 10.

---

## In this article

1. [Prerequisites](#prerequisites)
2. [Conceptual overview](#conceptual-overview)
3. [Arrays](#arrays)
4. [Lists](#lists)
5. [Side-by-side comparison](#side-by-side-comparison)
6. [Time and space complexity](#time-and-space-complexity)
7. [Choosing the right type](#choosing-the-right-type)
8. [Modern C# features (.NET 10)](#modern-c-features-net-10)
9. [Common pitfalls](#common-pitfalls)
10. [Performance tips](#performance-tips)
11. [See also](#see-also)

---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (`dotnet --version` &rarr; `10.0.x`)
- A basic understanding of C# syntax (variables, methods, generics)
- An editor such as Visual Studio, VS Code, or JetBrains Rider

Create a throwaway project to follow along:

```bash
dotnet new console -n CollectionsDemo
cd CollectionsDemo
dotnet run
```

---

## Conceptual overview

Both arrays and lists are **sequential collections**: items are stored
contiguously in memory and can be reached by a zero-based index.

```text
Index:     0     1     2     3     4
         ┌─────┬─────┬─────┬─────┬─────┐
Value:   │ 10  │ 20  │ 30  │ 40  │ 50  │
         └─────┴─────┴─────┴─────┴─────┘
```

The critical difference is **who controls the size**:

- An **array** has its size fixed the moment it is created. The runtime
  allocates exactly that many slots and the count never changes.
- A **`List<T>`** owns an internal array and manages its size for you. When
  the backing array fills up, it allocates a larger one (typically double the
  current capacity) and copies the elements across.

That single distinction drives every other difference you will see below.

---

## Arrays

An array is a fixed-length, strongly typed sequence of elements stored in a
single contiguous block on the managed heap.

### Declaration and initialization

```csharp
// 1. Declare with a size, elements default-initialized (0, null, false)
int[] scores = new int[5];

// 2. Inline initializer
int[] primes = { 2, 3, 5, 7, 11 };

// 3. Target-typed new (C# 9+)
int[] days = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

// 4. Collection expression (C# 12+, idiomatic in .NET 10)
int[] vowelsAscii = ['a', 'e', 'i', 'o', 'u'];

// 5. Multi-dimensional
int[,] matrix = new int[3, 3];
int[,] grid = { { 1, 2 }, { 3, 4 }, { 5, 6 } };
```

### Key characteristics

| Property | Behavior |
|---|---|
| Size | Fixed at construction; `Length` never changes |
| Memory | Single contiguous allocation, no overhead |
| Access | Direct pointer arithmetic &rarr; **O(1)** |
| Reference type | Yes &mdash; arrays are objects, passed by reference |
| Bounds-checked | Yes, the CLR throws `IndexOutOfRangeException` |

### Useful static helpers on `System.Array`

```csharp
int[] numbers = [5, 2, 8, 1, 9, 3];

Array.Sort(numbers);                          // in-place sort  &rarr; [1,2,3,5,8,9]
int idx   = Array.BinarySearch(numbers, 5);   // O(log n) on sorted array
int[] dbl = Array.ConvertAll(numbers, n => n * 2);
Array.Reverse(numbers);
int found = Array.Find(numbers, n => n > 4);
int[] many = Array.FindAll(numbers, n => n > 4);
```

---

## Lists

`List<T>` lives in `System.Collections.Generic` and is the go-to
general-purpose collection in .NET. It is a thin wrapper around an array that
grows automatically.

### Declaration and initialization

```csharp
// 1. Empty list
var cart = new List<string>();

// 2. With an initializer
var scores = new List<int> { 95, 82, 77 };

// 3. Pre-sized for performance (no re-allocations up to 1000 items)
var buffer = new List<byte>(capacity: 1000);

// 4. Collection expression (.NET 10 idiomatic)
List<string> colors = ["red", "green", "blue"];

// 5. From another sequence
var copy = new List<int>(Enumerable.Range(1, 10));
```

### Count vs Capacity

This is the single most important concept for understanding `List<T>`
performance.

```csharp
var list = new List<int>();

Console.WriteLine($"Count={list.Count}, Capacity={list.Capacity}");
// Count=0, Capacity=0

list.Add(1);
Console.WriteLine($"Count={list.Count}, Capacity={list.Capacity}");
// Count=1, Capacity=4   &larr; first Add allocates 4 slots

for (int i = 0; i < 10; i++) list.Add(i);
Console.WriteLine($"Count={list.Count}, Capacity={list.Capacity}");
// Count=11, Capacity=16  &larr; grew 4 &rarr; 8 &rarr; 16
```

- **`Count`** &mdash; the number of real items you have added.
- **`Capacity`** &mdash; the length of the internal array backing the list.

When `Count` reaches `Capacity`, the list allocates a new array roughly twice
as large and copies every element over. Pre-sizing with the constructor
avoids these copies when you know the final size in advance.

### Common operations

```csharp
var fruits = new List<string> { "apple", "banana" };

fruits.Add("cherry");                    // append           &rarr; O(1) amortized
fruits.Insert(0, "avocado");             // shift right      &rarr; O(n)
fruits.Remove("banana");                 // find + shift     &rarr; O(n)
fruits.RemoveAt(1);                      // shift            &rarr; O(n)
bool hasFig = fruits.Contains("fig");    // linear scan      &rarr; O(n)
fruits.Sort();                           // introsort        &rarr; O(n log n)
fruits.Reverse();                        // in-place reverse &rarr; O(n)
```

---

## Side-by-side comparison

| Feature | `T[]` (Array) | `List<T>` |
|---|---|---|
| Size | Fixed at creation | Grows and shrinks |
| Add / remove | Not supported | `Add`, `Insert`, `Remove`, `RemoveAt` |
| Length property | `Length` | `Count` |
| Reserved storage | Exactly `Length` slots | `Capacity` &ge; `Count` |
| Multi-dimensional | `int[,]`, `int[,,]` | Use `List<List<T>>` (jagged) |
| Memory overhead | None beyond the data | ~24 bytes header + spare capacity |
| Iteration | `for`, `foreach`, LINQ | `for`, `foreach`, LINQ |
| Underlying structure | Itself | Backed by an array |
| Typical use | Fixed data, hot paths | Everyday collections |

---

## Time and space complexity

| Operation | Array | `List<T>` | Notes |
|---|---|---|---|
| Random access `a[i]` | **O(1)** | **O(1)** | Index arithmetic |
| Search (unsorted) | O(n) | O(n) | Linear scan |
| Search (sorted) | O(log n) | O(log n) | `BinarySearch` |
| Append | &mdash; | **O(1)** amortized | Occasional O(n) resize |
| Insert at head | &mdash; | O(n) | Shifts every element right |
| Insert in middle | &mdash; | O(n) | Shifts tail right |
| Remove at head | &mdash; | O(n) | Shifts every element left |
| Remove at tail | &mdash; | **O(1)** | Decrement `Count` |
| Sort | O(n log n) | O(n log n) | Introsort |

**Space complexity** is O(n) for both, but `List<T>` may reserve up to ~2&times; as
much memory as it actually needs because of its doubling growth strategy.

---

## Choosing the right type

### Choose an array when&hellip;

- The size is known and **does not change** (days of the week, RGB channels,
  a chessboard, a fixed lookup table).
- You need **maximum performance** in a hot loop &mdash; JIT-optimized bounds
  checking and cache-friendly memory make arrays slightly faster.
- You are working with **multi-dimensional data** such as matrices or grids.
- You want the smallest possible memory footprint.

```csharp
// Perfect array use case: a fixed 12-element lookup
string[] monthNames =
[
    "Jan", "Feb", "Mar", "Apr", "May", "Jun",
    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
];
```

### Choose a `List<T>` when&hellip;

- The number of items is **not known upfront** or changes over time
  (shopping cart, audit log, query results).
- You need a rich API (`Add`, `Remove`, `Sort`, `Find`, `IndexOf`, `Contains`).
- You are using LINQ operators that return deferred sequences you will
  later materialize.

```csharp
// Perfect list use case: a dynamic event log
var events = new List<string>();
events.Add("User signed in");
events.Add("Cart item added");
events.RemoveAt(0);
```

> **Golden rule**: default to `List<T>` for everyday work; reach for arrays
> only when size is fixed or performance really matters.

---

## Modern C# features (.NET 10)

### Collection expressions

C# 12 introduced a unified literal syntax that works for arrays, lists,
`Span<T>`, `ImmutableArray<T>`, and any custom collection that opts in. It
has become the idiomatic way to create collections in .NET 10.

```csharp
int[]        primes   = [2, 3, 5, 7, 11];
List<string> colors   = ["red", "green", "blue"];
Span<byte>   header   = [0xDE, 0xAD, 0xBE, 0xEF];

// Spread operator: combine existing sequences
int[] first  = [1, 2, 3];
int[] second = [4, 5, 6];
int[] all    = [..first, ..second, 7, 8];   // [1,2,3,4,5,6,7,8]
```

### `params` collections (C# 13 / .NET 9+)

`params` is no longer restricted to arrays &mdash; any collection type works,
including `ReadOnlySpan<T>`, which avoids allocations entirely.

```csharp
// Zero-allocation params using ReadOnlySpan<T>
static int Sum(params ReadOnlySpan<int> values)
{
    int total = 0;
    foreach (var v in values) total += v;
    return total;
}

int result = Sum(1, 2, 3, 4, 5);   // no heap allocation
```

### `Span<T>` and `ReadOnlySpan<T>`

A `Span<T>` is a safe, stack-allocated view over a contiguous chunk of
memory &mdash; an array, a `stackalloc` block, or unmanaged memory. It gives you
array-like performance with zero-copy slicing.

```csharp
int[] numbers = [10, 20, 30, 40, 50];

Span<int> window = numbers.AsSpan(1, 3);   // [20, 30, 40] &mdash; no copy
window[0] = 99;                            // mutates the underlying array
// numbers is now [10, 99, 30, 40, 50]
```

Use `ReadOnlySpan<T>` when you need a read-only view, especially for strings
and parsing.

### `CollectionsMarshal` for advanced scenarios

For hot paths where you want to read or mutate a `List<T>` without paying
for bounds checks, use `CollectionsMarshal.AsSpan`:

```csharp
using System.Runtime.InteropServices;

var list = new List<int> { 1, 2, 3, 4, 5 };
Span<int> span = CollectionsMarshal.AsSpan(list);

for (int i = 0; i < span.Length; i++)
    span[i] *= 2;
// list is now [2, 4, 6, 8, 10]
```

> **Warning**: the span becomes invalid the moment the list is resized.
> Do not add or remove items while holding the span.

---

## Common pitfalls

### 1. IndexOutOfRangeException

```csharp
int[] arr = new int[3];
arr[3] = 10;   // throws &mdash; valid indices are 0, 1, 2
```

Use `arr.Length - 1` as the maximum index, or the `^1` index-from-end
operator to reach the last item:

```csharp
int last = arr[^1];   // equivalent to arr[arr.Length - 1]
```

### 2. Mutating a list while iterating

```csharp
foreach (var item in list)
    list.Remove(item);   // throws InvalidOperationException
```

Use `RemoveAll` with a predicate instead:

```csharp
list.RemoveAll(item => item.IsExpired);
```

### 3. Confusing `Length` with `Count`

```csharp
int[]      arr  = new int[5];
List<int>  list = [];

arr.Length;    // arrays use Length
list.Count;    // lists use Count
```

### 4. Forgetting that `List<T>` capacity never shrinks on remove

Adding 1&nbsp;000 items then removing 999 leaves the internal array at capacity
1024. Call `TrimExcess()` if memory pressure matters:

```csharp
list.TrimExcess();   // shrinks Capacity to Count (approximately)
```

### 5. Assuming arrays are covariant-safe

C# allows `string[]` to be assigned to `object[]`, but writes are checked at
runtime:

```csharp
object[] items = new string[3];
items[0] = 42;   // throws ArrayTypeMismatchException
```

Prefer `List<T>` or `IReadOnlyList<T>` to sidestep this legacy behavior.

---

## Performance tips

1. **Pre-size your list** when you know the final count.
   ```csharp
   var results = new List<Item>(capacity: expectedCount);
   ```
2. **Use `foreach` over `for`** on lists &mdash; the enumerator is struct-based
   and avoids bounds checks in most cases.
3. **Reach for `Span<T>`** for hot parsing and slicing work.
4. **Consider `Dictionary<TKey,TValue>`** if you search by key frequently.
   A linear `List.Find` on 10&nbsp;000 items is roughly 10&nbsp;000&times; slower than a
   dictionary lookup.
5. **Use `ImmutableArray<T>`** for data that must not change after construction
   &mdash; it is stored exactly like an array but exposes a read-only surface.

---

## See also

- [`System.Array` class](https://learn.microsoft.com/dotnet/api/system.array)
- [`List<T>` class](https://learn.microsoft.com/dotnet/api/system.collections.generic.list-1)
- [`Span<T>` and memory-efficient code](https://learn.microsoft.com/dotnet/standard/memory-and-spans/)
- [Collection expressions](https://learn.microsoft.com/dotnet/csharp/language-reference/operators/collection-expressions)
- [Choosing a collection](https://learn.microsoft.com/dotnet/standard/collections/selecting-a-collection-class)

---

<sub>Last reviewed: .NET 10 GA &middot; C# 14 language version</sub>

</div>