using System.Numerics;

namespace ConsoleApp1
{
    internal class Program
    {
        static Func<float, float, float> f1 = (y, x) => (y * y + x); // y  [0, 1.18]
        static Func<float, float> analf1 = y => 3 * (float)Math.Exp(y) - y * y - 2 * y - 2;
        static Func<float, float> pik1f1 = y => y * y * y / 3 + y + 1;  // y  [0, 0.09]
        static Func<float, float> pik2f1 = y => y * y * y * y / 12 + y * y * y / 3 + y * y / 2 + y + 1;// y  [0, 0.35]
        static Func<float, float> pik3f1 = y => y * y * y * y * y / 60 + y * y * y * y / 12 + y * y * y / 2 + y * y / 2 + y + 1; // y  [0, 0.48]
        static Func<float, float> pik4f1 = y => y * y * y * y * y * y / 360 + y * y * y * y * y / 60 + // y  [0, 0.68]
        y * y * y * y / 8 + y * y * y / 2 + y * y / 2 + y + 1;

        static Func<float, float, float> f2 = (y, x) => (y * y * y + 2 * x * y); // y  [0, 0.93]
        static Func<float, float> analf2 = y => (float)Math.Exp(y * y) - y * y / 2 - 0.5f;
        static Func<float, float> pik1f2 = y => y * y * y * y / 4 + y * y / 2 + 0.5f; // y  [0, 0.31]
        static Func<float, float> pik2f2 = y => (float)Math.Pow(y, 6) / 12 + y * y * y * y / 2 + y * y / 2 + 0.5f;// y  [0, 0.61]
        static Func<float, float> pik3f2 = y => (float)Math.Pow(y, 8) / 48 + (float)Math.Pow(y, 6) / 6 + y * y * y * y / 2 + y * y / 2 + 0.5f;// y  [0, 0.84]
        static Func<float, float> pik4f2 = y => (float)Math.Pow(y, 10) / 240 + (float)Math.Pow(y, 8) / 24 + // y  [0, 0.93]
        (float)Math.Pow(y, 6) / 6 + y * y * y * y / 2 + y * y / 2 + 0.5f; 

        static Func<float, float, float> f3 = (x, y) => (x * x + y * y);
        static Func<float, float> pik1f3 = x => x * x * x / 3;
        static Func<float, float> pik2f3 = x => (float)Math.Pow(x, 7) / 63 + x * x * x / 3;
        static Func<float, float> pik3f3 = x => (float)Math.Pow(x, 15) / 59535 + 2 * (float)Math.Pow(x, 11) / 2079 + 
        (float)Math.Pow(x, 7) / 63 + x * x * x / 3;
        static Func<float, float> pik4f3 = x => (float)Math.Pow(x, 31) / 109876902975 + 4 * (float)Math.Pow(x, 27) / 3341878155 + 
        622 * (float)Math.Pow(x, 23) / 10438212015 + 82 * (float)Math.Pow(x, 19) / 37328445 + 13 * (float)Math.Pow(x, 15) / 218295 + 
        2 * (float)Math.Pow(x, 11) / 2079 + (float)Math.Pow(x, 7) / 63 + x * x * x / 3;




        static void Main(string[] args)
        {
            List<Vector2> res1 = Euler.РешениеНеявное(f1, 0, 1, 1.18f, 0.01f);
            List<Vector2> res2 = Euler.РешениеЯвное(f1, 0, 1, 1.18f, 0.01f);
            List<Vector2> res3 = new(), res4= new(), res5 = new(), res6 = new(), res7 = new();
            foreach (var p in res1)
            {
                res3.Add(new(p.X, analf1(p.X)));
                res4.Add(new(p.X, pik1f1(p.X)));
                res5.Add(new(p.X, pik2f1(p.X)));
                res6.Add(new(p.X, pik3f1(p.X)));
                res7.Add(new(p.X, pik4f1(p.X)));
            }
            Console.WriteLine($"Y\t\t X неявный Эйлер Х явный Эйлер\t Х аналит\t X пикар 1\t X пикар 2\t X пикар 3\t X пикар 4");
            for (int i = 0; i < res1.Count; i++)
                Console.WriteLine($"{res1[i].X:f2}\t\t {res1[i].Y:f2}\t\t {res2[i].Y:f2}\t\t {res3[i].Y:f2}\t\t {res4[i].Y:f2}\t\t {res5[i].Y:f2}\t\t {res6[i].Y:f2}\t\t {res7[i].Y:f2}");
            Console.WriteLine("\n\n\n");
            res1 = Euler.РешениеНеявное(f2, 0, 0.5f, 1.18f, 0.01f);
            res2 = Euler.РешениеЯвное(f2, 0, 0.5f, 1.18f, 0.01f);
            res3 = new();
            res4 = new();
            res5 = new();
            res6 = new(); 
            res7 = new();
            foreach (var p in res1)
            {
                res3.Add(new(p.X, analf2(p.X)));
                res4.Add(new(p.X, pik1f2(p.X)));
                res5.Add(new(p.X, pik2f2(p.X)));
                res6.Add(new(p.X, pik3f2(p.X)));
                res7.Add(new(p.X, pik4f2(p.X)));
            }
            Console.WriteLine($"Y\t\t X неявный Эйлер Х явный Эйлер\t Х аналит\t X пикар 1\t X пикар 2\t X пикар 3\t X пикар 4");
            for (int i = 0; i < res1.Count; i++)
                Console.WriteLine($"{res1[i].X:f2}\t\t {res1[i].Y:f2}\t\t {res2[i].Y:f2}\t\t {res3[i].Y:f2}\t\t {res4[i].Y:f2}\t\t {res5[i].Y:f2}\t\t {res6[i].Y:f2}\t\t {res7[i].Y:f2}");
            
            Console.WriteLine("\n\n\n");
            res1 = Euler.РешениеНеявное(f3, 0, 0, 2.1f, 0.0001f);
            res2 = Euler.РешениеЯвное(f3, 0, 0, 2.1f, 0.0001f);
            res3 = new();
            res4 = new();
            res5 = new();
            res6 = new();
            res7 = new();
            foreach (var p in res1)
            {
                res4.Add(new(p.X, pik1f3(p.X)));
                res5.Add(new(p.X, pik2f3(p.X)));
                res6.Add(new(p.X, pik3f3(p.X)));
                res7.Add(new(p.X, pik4f3(p.X)));
            }
            Console.WriteLine($"Y\t\t X неявный Эйлер Х явный Эйлер\t X пикар 1\t X пикар 2\t X пикар 3\t X пикар 4");
            for (int i = 0; i < res1.Count; i++)
                Console.WriteLine($"{res1[i].X}\t\t {res1[i].Y:f2}\t\t {res2[i].Y:f2}\t\t {res4[i].Y:f2}\t\t {res5[i].Y:f2}\t\t {res6[i].Y:f2}\t\t {res7[i].Y:f2}");

        }
    }
}


/*
  
class UDESolver:
    def __init__(self, x_start, y_start, x_max, step, f, f_approximation_number_s):
        if (x_start < x_max) != (step > 0):
            raise ValueError('Ошибка в шаге')

        self.x_start = x_start
        self.y_start = y_start
        self.x_max = x_max
        self.step = step
        self.f = f
        self.f_approximation_number_s = f_approximation_number_s
        self.cmp_func = lambda x1, x2: x1 < x2 + EPS

    def reverse_move(self):
        self.step *= -1
        self.x_max *= -1
        self.cmp_func = lambda x1, x2: x1 > x2 - EPS

    def x_range(self):
        result = []
        x = self.x_start
        while self.cmp_func(x, self.x_max):
            result.append(x)
            x += self.step
        return result

    def solve_euler(self):
        result = []
        x, y = self.x_start, self.y_start

        while self.cmp_func(x, self.x_max):
            result.append(y)

            y = y + self.step * self.f(x, y)
            x += self.step

        return result

    def solve_runge_kutta(self):
        a = 0.5
        result = []
        x, y = self.x_start, self.y_start

        while self.cmp_func(x, self.x_max):
            result.append(y)

            k1 = self.f(x, y)
            k2 = self.f(x + self.step / (2 * a), y + self.step * k1 / (2 * a))
            y += self.step * ((1 - a) * k1 + a * k2)
            x += self.step

        return result

    def solve_picar(self, approx):
        result = []
        x, y = self.x_start, self.y_start

        while self.cmp_func(x, self.x_max):
            result.append(y)
            x += self.step
            y = self.f_approximation_number_s(x, approx)

        return result


def function(x, u):
    return x * x + u * u


def function_approximation_number_s(x, s):
    first_sum = 0
    for i in range(1, s + 1):
        chisl = pow(x, pow(2, i + 1) - 1)
        znam = 1
        for k in range(-1, i - 2 + 1):
            znam *= (pow((pow(2, i - k) - 1), pow(2, k + 1)))
        first_sum += (chisl / znam)

    second_sum = 0
    for i in range(3, s + 1):
        chisl = pow(x, pow(2, i + 1) - 1)
        znam = 1
        for k in range(-1, i - 2 + 1):
            znam *= (pow((pow(2, i - k) - 1), pow(2, k + 1)))
        first_mult = chisl / znam

        second_mult = 0
        for j in range(i + 1, s + 1):
            chisl = pow(x, pow(2, j + 1) - 1)
            znam = 1
            for p in range(-1, j - 2 + 1):
                znam *= (pow((pow(2, j - p) - 1), pow(2, j + 1)))
            second_mult += (chisl / znam)
        second_sum += (first_mult * second_mult)

    second_sum *= 2

    return first_sum + second_sum

# x = 1
# print(f'1: {fd1(x)} {function_approximation_number_s(x, 1)}')
# print(f'2: {fd2(x)} {function_approximation_number_s(x, 2)}')
# print(f'3: {fd3(x)} {function_approximation_number_s(x, 3)}')
# print(f'4: {fd4(x)} {function_approximation_number_s(x, 4)}')

def draw_plots(table):
    plt.figure(figsize=(30, 10))
    x = table.index
    for column_name in table.columns:
        y = table[column_name]
        plt.plot(x, y, label=column_name)

    plt.legend()
    plt.show()

def main():
    step_accuracy = 4
    x_max = 1.77

    x_start = 0
    y_start = 0
    round_accuracy = 2
    step = float(f'1e-{step_accuracy}')
    show_each = 100

    pd.set_option('display.max_rows', None)
    pd.set_option('display.max_columns', None)
    pd.set_option('display.max_colwidth', None)
    pd.set_option('display.float_format', lambda x: f'%.{round_accuracy}f' % x)

    solver = UDESolver(x_start, y_start, x_max, step, function, function_approximation_number_s)

    table = pd.DataFrame(index=solver.x_range())
    table['x'] = solver.x_range()
    table = table.set_index('x')
    table['Euler'] = solver.solve_euler()
    table['Runge-Kutta'] = solver.solve_runge_kutta()
    for i in range(4):
        table[f"Picard, {i + 1}"] = solver.solve_picar(i + 1)


    print(table.iloc[::show_each, :])
    # draw_plots(table)


    solver.reverse_move()

    table2 = pd.DataFrame(index=solver.x_range())
    table2['x'] = solver.x_range()
    table2 = table2.set_index('x')
    table2['Euler'] = solver.solve_euler()
    table2['Runge-Kutta'] = solver.solve_runge_kutta()
    for i in range(4):
        table2[f"Picard, {i + 1}"] = solver.solve_picar(i + 1)

    # print(table2)
    # draw_plots(table2)

    full_table = pd.concat([table, table2], sort=True, axis=0)
    full_table = full_table.sort_index(ascending=True)

    # print(full_table)
    draw_plots(full_table)



if __name__ == "__main__":
    main()


*/