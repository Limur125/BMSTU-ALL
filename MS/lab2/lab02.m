% Вариант 13

X = [-10.82,-9.27,-9.65,-9.36,-9.27,-11.25,-9.89,-9.26,-11.15,-8.90,-11.02,-8.28,-9.18,-10.16,-10.59,-10.82,-9.05,-9.47,-10.98,-11.50,-8.64,-10.86,-10.76,-11.49,-11.09,-9.33,-9.32,-9.66,-8.79,-10.54,-9.12,-10.40,-8.59,-10.22,-9.06,-10.59,-10.60,-10.25,-9.35,-11.44,-9.81,-9.32,-9.95,-9.33,-10.64,-9.45,-10.99,-10.15,-10.39,-10.36,-10.49,-11.67,-10.00,-10.87,-11.11,-9.68,-10.77,-9.13,-8.62,-10.33,-11.36,-10.24,-9.41,-11.05,-10.15,-9.35,-11.45,-9.87,-10.41,-10.11,-10.84,-11.48,-7.77,-10.79,-9.88,-10.70,-9.07,-9.47,-10.15,-9.93,-11.52,-9.04,-10.93,-10.13,-9.56,-11.39,-9.79,-9.19,-11.09,-9.86,-10.67,-10.26,-9.07,-10.53,-11.24,-10.16,-11.33,-8.76,-8.88,-10.53,-10.12,-8.98,-9.84,-9.90,-10.13,-9.32,-9.31,-9.99,-8.55,-11.64,-11.32,-10.51,-11.71,-10.50,-10.50,-12.20,-11.68,-10.45,-7.88,-10.84];

gamma = 0.9;
n = length(x);

% 1
% Точечная оценка мат. ожидания и дисперсии
mu = expectation(X);
s2 = variance(X);
fprintf('mu = %.2f\n', mu); 
fprintf('S^2 = %.2f\n\n', s2);

% Нижняя и верхняя граница для mu
% tinv(a, n) - квантиль уровня a распределения Стьюдента с n степенями свободы.
mu_low = mu + (sqrt(s2) * tinv((1 - gamma) / 2, n - 1)) / sqrt(n);
mu_high = mu + (sqrt(s2) * tinv((1 + gamma) / 2, n - 1)) / sqrt(n);
fprintf('mu_low = %.2f\n', mu_low);
fprintf('mu_high = %.2f\n\n', mu_high);

% Нижняя и верхняя граница для s2
% chi2inv(a, n) - квантиль уровня a распределения хи квадрат с n степенями свободы.
sigma_low  = ((n - 1) * s2) / chi2inv((1 + gamma) / 2, n - 1);
sigma_high = ((n - 1) * s2) / chi2inv((1 - gamma) / 2, n - 1);
fprintf('sigma_low = %.2f\n', sigma_low);
fprintf('sigma_high = %.2f\n\n', sigma_high);

% Вычислить мат. ожидание и дисперсию
MX = mean(X);
DX = var(X);
fprintf("MX = %.2f\n", MX);   
fprintf("DX = %.2f\n", DX);

% 3
n_array = zeros([1 n]);
mu_array = zeros([1 n]);
mu_low_array = zeros([1 n]);
mu_high_array = zeros([1 n]);
s2_array = zeros([1 n]);
s2_low_array = zeros([1 n]);
s2_high_array = zeros([1 n]);

for i = 1:n
    n_array(i) = i;

    mu_i = find_mu(X(1:i));
    mu_array(i) = mu_i;
    s_sqr_i = find_s_sqr(X(1:i));
    s2_array(i) = s_sqr_i;

    mu_low_array(i) = find_mu_low(mu_i, s_sqr_i, i, gamma);
    mu_high_array(i) = find_mu_high(mu_i, s_sqr_i, i, gamma);
    
    s2_low_array(i) = find_sigma_sqr_low(s_sqr_i, i, gamma);
    s2_high_array(i) = find_sigma_sqr_high(s_sqr_i, i, gamma);
end

mu_const = mu * ones(n);
s2_const = s2 * ones(n);

% a
plot(n_array, mu_const, n_array, mu_array, ...
    n_array, mu_low_array, n_array, mu_high_array);
xlabel('n');
ylabel('y');
xlim([10 n]);
legend('$\hat \mu(\vec x_N)$', '$\hat \mu(\vec x_n)$', ...
    '$\underline{\mu}(\vec x_n)$', '$\overline{\mu}(\vec x_n)$', ...
    'Interpreter', 'latex', 'FontSize', 14);
figure;


% b
plot(n_array, s2_const, n_array, s2_array, ...
    n_array, s2_low_array, n_array, s2_high_array);
xlabel('n');
ylabel('z');
xlim([10 n]);
legend('$\hat S^2(\vec x_N)$', '$\hat S^2(\vec x_n)$', ...
   '$\underline{\sigma}^2(\vec x_n)$', '$\overline{\sigma}^2(\vec x_n)$', ...
   'Interpreter', 'latex', 'FontSize', 14);

% functions
function [mu] = find_mu(X)
    mu = mean(X);
end

function [s_sqr] = find_s_sqr(X)
    s_sqr = var(X);
end

% tinv(a, n) - квантиль уровня a распределения Стьюдента с n степенями свободы.
function [mu_low] = find_mu_low(mu, s_sqr, n, gamma)
    mu_low = mu - sqrt(s_sqr) * tinv((1 + gamma) / 2, n - 1) / sqrt(n);
end

function [mu_high] = find_mu_high(mu, s_sqr, n, gamma)
    mu_high = mu + sqrt(s_sqr) * tinv((1 + gamma) / 2, n - 1) / sqrt(n);
end

%chi2inv(a, n) - квантиль уровня a распределения хи квадрат с n степенями свободы.
function [sigma_sqr_low] = find_sigma_sqr_low(s_sqr, n, gamma)
    sigma_sqr_low = (n - 1) * s_sqr / chi2inv((1 + gamma) / 2, n - 1);
end

function [sigma_sqr_high] = find_sigma_sqr_high(s_sqr, n, gamma)
    sigma_sqr_high = (n - 1) * s_sqr / chi2inv((1 - gamma) / 2, n - 1);
end


% Вспомогательные функции
% Точечная оценка мат. ожидания.
function mu = expectation(x)
    mu = sum(x) / length(x);
end

% Точечная оценка дисперсии.
function s2 = variance(x)
   n = length(x);
   mu = expectation(x);
   s2 = sum((x - mu).^2) / (n - 1);
end