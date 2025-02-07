function lab1()
    clc;
    debugMode = 1;
    findMax = 0;

    matr = [
      9   11    3    6    6;
      10   9   11    5    6;
      8   10    5    6    4;
      6    8   10    4    9;
      11  10    9    8    7];

    disp('5 вариант. Матрица:');
    disp(matr);

    C = matr;

    if findMax == 1
        C = convertToMin(matr);
    end

    if debugMode == 1
      disp('Матрица после приведения к задаче минимизации:');
      disp(C);
    end

    C = SubtractMinFromCols(C);
    if (debugMode == 1)
        disp('Вычесть минимум из каждого столбца');
        disp(C);
    end

    C = SubtractMinFromRows(C);
    if (debugMode == 1)
        disp('Вычесть минимум из каждой строки');
        disp(C);
    end

    snn = getSNN_Init(C);
    k = length(find(snn(:,2) > 0));
    if debugMode == 1
        disp('Начальная СНН');
        print_SNN(C, snn);
    end
    if debugMode == 1
        fprintf('Число нулей в построенной СНН: k = %d\n\n', k);
    end
    [r, c] = size(C);

    iteration = 1;
    while k < c
        if debugMode == 1
            fprintf('---------------- Итерация №%d ----------------\n', iteration);
        end

        shtrih = zeros(r * c, 2);   % позиции 0'
        b = 1;
        selectedColumns = snn(:,2);
        selectedRows = zeros(1, r);
        selection = getSelection(r, c, selectedColumns);
        if debugMode == 1
            disp('Результат выделения столбцов, в которых стоит 0*:');
            printMarkedMatr(C, snn, shtrih, selectedColumns, selectedRows);
        end
        flag = true;
        shp = [-1 -1];
        while flag
            if debugMode == 1
                disp('Поиск 0 среди невыделенных элементов');
            end

            shp = findShtrih(C, selection);
            if shp(1) == -1
                C = updateMatrNoZero(C, r, c, selection, selectedRows, selectedColumns);
                if debugMode == 1
                    disp('Т.к. среди невыделенных элементов нет нулей, матрица была преобразована:');
                    printMarkedMatr(C, snn, shtrih, selectedColumns, selectedRows);
                end
                shp = findShtrih(C, selection);
            end

            shtrih(b,:) = shp;
            b = b+1;
            if debugMode == 1
                disp('Матрица с найденным 0-штрих');
                printMarkedMatr(C, snn, shtrih, selectedColumns, selectedRows);
            end

            zeroStarInRow = getZeroStarInRow(shp, snn);
            if isempty(zeroStarInRow)
                flag = false;
            else
                % снять выделение со столбца с 0*
                selection(:, zeroStarInRow(2)) = selection(:, zeroStarInRow(2)) - 1;
                selectedColumns(zeroStarInRow(2)) = 0;

                % перенести выделение на строку с 0'
                selection(zeroStarInRow(1), :) = selection(zeroStarInRow(1), :) + 1;
                selectedRows(zeroStarInRow(1)) = 1;
                if debugMode == 1
                    disp('Т.к. в одной строке с 0-штрих есть 0*, было переброшено выделение:');
                    printMarkedMatr(C, snn, shtrih, selectedColumns, selectedRows);
                end
            end
         end


         if debugMode == 1
            disp('L-цепочка: ');
         end

         [shtrih, snn] = createL(r, c, shp, shtrih, snn);

         k = length(find(snn(:,2) > 0));
         if debugMode == 1
            disp('Текущая СНН:');
            print_SNN(C, snn);
            fprintf('Итого, k = %d\n', k);
         end

         iteration = iteration + 1;
         disp('----------------------------------------------');
    end

    disp('Конечная СНН:');
    print_SNN(C, snn);

    disp('X =');
    print_X(snn);

    fOpt = getFOpt(matr, snn);
    fprintf("Результат = %d\n", fOpt);
end

function matr = convertToMin(matr)
  maxElem = max(matr, [], "all");
  matr = (-1) * matr + maxElem;
end
function matr = SubtractMinFromCols(matr)
  minElemArr = min(matr);
  for i = 1 : length(matr)
    matr(:, i) = matr(:, i) - minElemArr(i);
  end
end

function matr = SubtractMinFromRows(matr)
  minElemArr = min(matr, [], 2);
  for i = 1 : length(minElemArr)
    matr(i, :) = matr(i, :) - minElemArr(i);
  end
end

function SNN = getSNN_Init(matr)
    [m, n] = size(matr);
    SNN = zeros(n, 2);
    for i = 1: n
        for j = 1 : m
            if matr(j, i) == 0
                k = 1;
                while SNN(k, 1) ~= j && SNN(k, 2) ~= i && k < n
                    k = k + 1;
                end
                if (k == n)
                    SNN(i, 1) = j;
                    SNN(i, 2) = i;
                end
            end
        end
    end
end

function [] = print_SNN(matr, SNN)
    [r, c] = size(matr);
    fprintf("\n");
    for i = 1 : r
        for j = 1 : c
            inds = [i, j];
            f = find(ismember(SNN,inds, "rows"), 1);
            if (f > 0) 
                fprintf("\t%d*", matr(i, j));
            else
                fprintf("\t%d", matr(i, j));
            end
        end
        fprintf("\n");
    end
    fprintf("\n");
end

function [] = print_X(SNN)
    [r, c] = size(SNN);
    fprintf("\n");
    for i = 1 : r
        for j = 1 : r
            inds = [i, j];
            f = find(ismember(SNN,inds, "rows"), 1);
            if (f > 0) 
                fprintf("\t1");
            else
                fprintf("\t0");
            end
        end
        fprintf("\n");
    end
    fprintf("\n");
end

function [selection] = getSelection(r, c, selectedColumns)
  selection = zeros(r, c);
  for i = 1 : c
    if selectedColumns(i) > 0
      selection(:, i) = selection(:, i) + 1;
    end
  end
end

function [] = printMarkedMatr(matr, SNN, shtrih, selectedCols, selectedRows)
  [r,c] = size(matr);

  for i = 1 : r
    if selectedRows(i) > 0
      fprintf("+")
    end

    for j = 1 : c
        fprintf("\t%d", matr(i, j));
        inds = [i, j];
        f1 = find(ismember(SNN, inds, "rows"), 1);
        f2 = find(ismember(shtrih, inds, "rows"), 1);
        if (f1 > 0) 
            fprintf("*");
        elseif(f2 > 0)
            fprintf("'")
        end
    end

    fprintf('\n');
  end

  for i = 1 : c
    if selectedCols(i) > 0
      fprintf("\t+")
    else
      fprintf(" \t")
    end
  end
  fprintf('\n\n');
end

% Найти первый нулевой элемент среди невыделенных, в одной строке с которым не
% стоит 0*
function [streakPnt] = findShtrih(matr, selection)
  streakPnt = [-1 -1];
  [numRows,numCols] = size(matr);
  for i = 1 : numCols
    for j = 1 : numRows
       if selection(j, i) == 0 && matr(j, i) == 0
        streakPnt(1) = j;
        streakPnt(2) = i;
        return;
       end
    end
  end
end

% Изменить матрицу в случае, если среди невыделенных элементов нет нуля
function [matr] = updateMatrNoZero(matr, numRows, numCols, selection, selectedRows, selectedColumns)
  h = 1e5; % Наименьший элемент среди невыделенных
  for i = 1 : numCols
    for j = 1 : numRows
      if selection(j, i) == 0 && matr(j, i) < h
        h = matr(j, i);
      end
    end
  end

  for i = 1 : numCols
    if selectedColumns(i) == 0
      matr(:, i) = matr(:, i) - h;
    end
  end
  for i = 1 : numRows
    if selectedRows(i) > 0
      matr(i, :) = matr(i, :) + h;
    end
  end
end

% Найти 0* в той же строке, что и 0'
function [zeroStarInRow] = getZeroStarInRow(shp, snn)
  j = shp(1);
  i = find(snn(:,1)==j);
  zeroStarInRow = [snn(i, 1), snn(i, 2)];
end

% Построить L-цепочку
function [shtrih, snn] = createL(r, c, shp, shtrih, snn)
  i = shp(1);
  j = shp(2);
  fprintf("[%d, %d]\n", i, j);
  while true
    inds = [i, j];
    k = find(ismember(shtrih, inds, "rows"), 1);
    shtrih(k, :) = [0, 0];
    k = find(snn(:,2)==j);

    if isempty(k)
        snn(j,:) = inds;
        break;
    end
    s = snn(k,:);
    fprintf("[%d, %d] -> ", s(1), s(2));
    snn(j,:) = inds;
    k = find(shtrih(:,1)==s(2));
    inds = shtrih(k,:);
    i = inds(1);
    j = inds(2);
    fprintf("[%d, %d]", i, j);
    fprintf("\n");
  end
end

function [fOpt] = getFOpt(matr, snn)
  fOpt = 0;
  [r,~] = size(snn);

  for i = 1 : r
      o = snn(i,:);
      k = o(1);
      l = o(2);
      fOpt = fOpt + matr(k, l);
  end
end

