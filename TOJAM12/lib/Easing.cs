using System;
using Microsoft.Xna.Framework;

namespace TOJAM12
{
	public class Easing<T>
	{
		public T current;
		T start;
		T finish;
		float speed;

		Calculator<T> calc;

		public Easing(T start, T finish, float speed)
		{
			this.start = start;
			this.finish = finish;
			this.speed = speed;
			this.current = start;

			if (typeof(T) == typeof(Vector2))
			{
				calc = Activator.CreateInstance(typeof(Vector2Calculator)) as Calculator<T>;
			}
			else if (typeof(T) == typeof(float))
			{
				calc = Activator.CreateInstance(typeof(FloatCalculator)) as Calculator<T>;
			}
		}

		public T Update(GameTime t, bool condition)
		{
			T target = condition ? finish : start;
			float scale = this.speed * (float)t.ElapsedGameTime.TotalSeconds;
			this.current = calc.Add(current, calc.Mul(calc.Sub(target, current), scale));
			return this.current;
		}
	}

	public class Jitter<T>
	{
		public T current;
		T start;
		T finish;
		Random r;

		Calculator<T> calc;

		public Jitter(T start, T finish)
		{
			this.start = start;
			this.finish = finish;
			this.current = start;
			this.r = new Random();

			if (typeof(T) == typeof(Vector2))
			{
				calc = Activator.CreateInstance(typeof(Vector2Calculator)) as Calculator<T>;
			}
			else if (typeof(T) == typeof(float))
			{
				calc = Activator.CreateInstance(typeof(FloatCalculator)) as Calculator<T>;
			}
		}

		public T Update(GameTime t)
		{
			float scale = (float)r.NextDouble();
			this.current = calc.Add(calc.Mul(start, scale), calc.Mul(finish, (1 - scale)));
			return this.current;
		}
	}

	public interface Calculator<T>
	{
		T Sub(T a, T b);
		T Add(T a, T b);
		T Mul(T a, float scale);
	}

	struct Vector2Calculator : Calculator<Vector2>
	{
		public Vector2 Sub(Vector2 a, Vector2 b) { return a - b; }
		public Vector2 Add(Vector2 a, Vector2 b) { return a + b; }
		public Vector2 Mul(Vector2 a, float s) { return a * s; }
	}

	struct FloatCalculator : Calculator<float>
	{
		public float Sub(float a, float b) { return a - b; }
		public float Add(float a, float b) { return a + b; }
		public float Mul(float a, float s) { return a * s; }
	}
}
