# Overview

This project contains a collection of math equations that allow points to be manipulated in various ways.

---

## Rotation

This project allows you to rotate a point around a pivot point.  
This is accomplished using basic trigonometric functions such as `sin` and `cos`.

---

## Sine and Cosine

Assuming you are familiar with right triangles, this section explains `sin` and `cos`.

These functions describe relationships between angles and side lengths in right triangles and circles (usually the unit circle).

Let θ (theta) represent the angle being measured. This is typically the angle used to determine a point’s position.

A **unit circle** is a circle centered at `(0, 0)` with a radius of `1`.

Using a right triangle:

- `cos(θ) = adjacent / hypotenuse`
- `sin(θ) = opposite / hypotenuse`

When inputting θ into these functions, the value is **not in degrees**.  
Trigonometric functions use **radians**.

Because the unit circle has a radius (hypotenuse) of `1`, the equations simplify to:

- `cos(θ) = adjacent`
- `sin(θ) = opposite`

---

## Radians

Radians measure angles using arc length rather than arbitrary degree values.

One radian is defined as the angle created when the arc length equals the radius of the circle.

### Common Conversions

- 90° = π / 2  
- 180° = π  
- 360° = 2π  

---

## Position Equation

To get the position of a point on a circle using an angle θ:

### Coordinates on the Unit Circle

- `cos(θ)` gives the **X coordinate** (Remember `cos` uses the adjacent side which is the bottom of the triangle)
- `sin(θ)` gives the **Y coordinate** (`sin` uses the opposite side or the leg which is the height of the triangle)

This produces the point: ```(cos(θ), sin(θ))```

### Converting from the Unit Circle

Since these values come from the unit circle, they must be scaled and offset to match the actual position.

1. Scale by the desired distance from the pivot: ```(cos(θ), sin(θ)) * distance``` (distance is just the pythagorean theorem as we all know)
2. Offset by the original position:  ```original_position + (cos(θ), sin(θ)) * distance```

This produces the final rotated point.

---

## Summary

By using `sin` and `cos` with radians, points can be rotated around a pivot by calculating their position on a circle and applying the correct offset.
