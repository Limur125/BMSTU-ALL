function hungarian_method()
    clc;
    findMax = false;
    debugMode = true;

    matr = [
        4 7 1 5 5;
        6 8 3 7 6;
        6 4 5 7 7;
        4 2 3 4 9;
        8 1 8 3 8
    ];

    disp('Матрица стоимостей (9 вариант):');
    disp(matr);

    C = matr;

    if findMax == true
        C = convertToMinimizationProblem(matr);

        if debugMode == true
            disp('Матрица стоимостей после сведения к задаче минимизации:');
            disp(C);
        end
    end

    C = updateColumns(C);
    if debugMode == true
        disp("Результат вычитания наименьшего элемента по столбцам:");
        disp(C);
    end

    C = updateRows(C);
    if debugMode == true
        fprintf("Результат вычитания наименьшего элемента по строкам:\n");
        disp(C);
    end

    [numRows, numCols] = size(C);

    matrSIZ = makeSIZ(C);
    if debugMode == true
        fprintf('Начальная СНН:\n');
        printSIZ(C, matrSIZ);
    end

    k = sum(sum(matrSIZ));
    if debugMode == true
        fprintf('\nЧисло нулей в построенной СНН: k = %d\n', k);
    end 

    iteration = 1;
    while k < numCols
        if debugMode == true
            fprintf('\nk < n = %d ==> СНН нужно улучшать\n', numCols);
            fprintf('_____________________________ Итерация №%d ______________________________\n', iteration);
        end

        matrStreak = zeros(numRows, numCols);
        selectedColumns = sum(matrSIZ);
        selectedRows = zeros(numRows);
        selection = makeSelection(numRows, numCols, selectedColumns);
        
        if debugMode == true
            fprintf('\nРезультат выделения столбцов, в которых стоит 0*:\n');
            printMarkedMatr(C, matrSIZ, matrStreak, selectedColumns, selectedRows);
        end

        kChanged = false;
        streakPoint = [-1 -1];
        while kChanged == false 
            streakPoint = findStreak(C, selection);
            if streakPoint(1) == -1
                if debugMode == true
                    fprintf("\nСреди невыделенных элементов нет 0, преобразуем матрицу:\n");
                end

                C = updateMatrNoZero(C, numRows, numCols, selection, selectedRows, selectedColumns);

                if debugMode == true
                    printMarkedMatr(C, matrSIZ, matrStreak, selectedColumns, selectedRows);
                end

                streakPoint = findStreak(C, selection);
            end
        
            matrStreak(streakPoint(1), streakPoint(2)) = 1;
            if debugMode == true
                fprintf("\nСреди невыделенных элементов есть 0, отметим 0':\n");
                printMarkedMatr(C, matrSIZ, matrStreak, selectedColumns, selectedRows);
            end

            zeroStarInRow = getZeroStarInRow(streakPoint, numCols, matrSIZ);
            if zeroStarInRow(1) == -1
                [matrStreak, matrSIZ] = makeLChain(numRows, numCols, streakPoint, matrStreak, matrSIZ, debugMode);
                kChanged = true;
            else
                % снять выделение со столбца с 0*
                selection(:, zeroStarInRow(2)) = selection(:, zeroStarInRow(2)) - 1;
                selectedColumns(zeroStarInRow(2)) = 0;

                % перенести выделение на строку с 0'
                selection(zeroStarInRow(1), :) = selection(zeroStarInRow(1), :) + 1; 
                selectedRows(zeroStarInRow(1)) = 1;
                if debugMode == true
                    fprintf("\nВ одной строке с 0' есть 0*, перебросим выделение со столбца на строку:\n");
                    printMarkedMatr(C, matrSIZ, matrStreak, selectedColumns, selectedRows);
                end
            end
        end

        k = sum(sum(matrSIZ));
        if debugMode == true
            fprintf("\nВ пределах L-цепочки 0* заменем на 0, а 0' на 0*:\n");
            printSIZ(C, matrSIZ); 
            fprintf('\nЧисло нулей в построенной СНН: k = %d\n', k);
        end
        
        iteration = iteration + 1;
    end

    if debugMode == true
        fprintf("\nКонечная СНН:\n");
        printSIZ(C, matrSIZ);
    end

    fprintf("\nX: \n");
    disp(matrSIZ);

    fOpt = getFOpt(matr, matrSIZ);
    fprintf("f_opt = %d", fOpt);
end 

% Найти первый нулевой элемент среди невыделенных, в одной строке с которым не стоит 0*
function [streakPoint] = findStreak(matr, selection) 
    streakPoint = [-1 -1];
    [numRows, numCols] = size(matr);
    for i = 1 : numCols
        for j = 1 : numRows
           if selection(j, i) == 0 && matr(j, i) == 0 
                streakPoint(1) = j;
                streakPoint(2) = i;
                return;
           end
        end 
    end
end

function [] = printSIZ(matr, matrSIZ)
    [numRows, numCols] = size(matr);

    for i = 1 : numRows
        for j = 1 : numCols
            if matrSIZ(i, j) == 1
                fprintf("\t%d*", matr(i, j));
            else
                fprintf("\t%d", matr(i, j));
            end
        end
        fprintf("\n");
    end
end

function [] = printMarkedMatr(matr, matrSIZ, matrStreak, selectedCols, selectedRows)
    [numRows, numCols] = size(matr);

    for i = 1 : numRows
        if selectedRows(i) == 1
            fprintf("+");
        end

        for j = 1 : numCols
            if matrSIZ(i, j) == 1 
                fprintf("\t%d*\t", matr(i, j));
            elseif matrStreak(i, j) == 1
                fprintf("\t%d'\t", matr(i, j));
            else
                fprintf("\t%d\t", matr(i, j));
            end
        end
        fprintf("\n");
    end

    for i = 1 : numCols
        if selectedCols(i) == 1
            fprintf("\t+\t");
        else 
            fprintf(" \t\t");
        end 
    end
    fprintf("\n");
end

% Сведение задачи максимизации к задаче минимизации
function matr = convertToMinimizationProblem(matr)
    maxElem = max(max(matr));
    matr = matr * (-1) + maxElem;
end

% Нахождение наименьшего элемента в каждом столбце матрицы C 
% и вычитание его из соответствующего столбца
function matr = updateColumns(matr)
    minElemArr = min(matr);
    for i = 1 : length(minElemArr)
        matr(:, i) = matr(:, i) - minElemArr(i);
    end
end

% Нахождение наименьшего элемента в каждой строке матрицы C 
% и вычитание его из соответствующей строки
function matr = updateRows(matr)
    minElemArr = min(matr, [], 2);
    for i = 1 : length(minElemArr)
        matr(i, :) = matr(i, :) - minElemArr(i);
    end
end

% Построение начальной СНН
function matrSIZ = makeSIZ(matr)
    [numRows, numCols] = size(matr);
    matrSIZ = zeros(numRows, numCols);

    for i = 1 : numRows
        for j = 1 : numCols
            if matr(i, j) == 0
                count = 0;
                for k = 1 : numCols
                   count = count + matrSIZ(i, k);
                end
                for k = 1 : numRows
                   count = count + matrSIZ(k, j);
                end
                if count == 0 
                    matrSIZ(i, j) = 1;
                end 
            end
        end 
    end
end

% Выделение столбцов, в которых стоит 0*
function [selection] = makeSelection(numRows, numCols, selectedColumns)
    selection = zeros(numRows, numCols);
    for j = 1 : numCols
        if selectedColumns(j) == 1 
            selection(:, j) = selection(:, j) + 1;
        end 
    end
end

% Изменить матрицу в случае, если среди невыделенных элементов нет нуля
function [matr] = updateMatrNoZero(matr, numRows, numCols, selection, selectedRows, selectedColumns)
    h = -1;
    for i = 1 : numCols
        for j = 1 : numRows
            if selection(j, i) == 0 && (matr(j, i) < h || h == -1)
                h = matr(j, i);
            end
        end 
    end
    fprintf("h = %d\n", h);

    for i = 1 : numCols
        if selectedColumns(i) == 0
            matr(:, i) = matr(:, i) - h;
        end 
    end
    for i = 1 : numRows
        if selectedRows(i) == 1
            matr(i, :) = matr(i, :) + h;
        end 
    end
end

% Найти 0* в той же строке, что и 0'
function [zeroStarInRow] = getZeroStarInRow(streakPoint, numCols, matrSIZ)
    j = streakPoint(1);
    zeroStarInRow = [-1 -1];
    for i = 1 : numCols
       if matrSIZ(j, i) == 1
           zeroStarInRow(1) = j;
           zeroStarInRow(2) = i;
           break
       end 
    end
end

% Построить L-цепочку
function [matrStreak, matrSIZ] = makeLChain(numRows, numCols, streakPoint, matrStreak, matrSIZ, debugMode)
    if debugMode == true
        fprintf("Построенная L-цепочка:");
    end

    i = streakPoint(1);
    j = streakPoint(2);
    while i > 0 && j > 0 && i <= numRows && j <= numCols
        % Снять '
        matrStreak(i, j) = 0;
        % Поставить *
        matrSIZ(i, j) = 1;
        
        if debugMode == true
            fprintf("[%d, %d] ", i, j);
        end

        % Дойти до 0* по столбцу от 0'
        row = 1;
        while row <= numRows && (matrSIZ(row, j) ~= 1 || row == i)
            row = row + 1;
        end

        if row <= numRows
            % Дойти до 0' по строке от 0*
            col = 1;
            while col <= numCols && (matrStreak(row, col) ~= 1 || col == j)
                col = col + 1;
            end

            if col <= numCols
                matrSIZ(row,j) = 0;

                if debugMode == true
                    fprintf("-> [%d, %d] -> ", row, j);
                end
            end
            j = col;
        end
        i = row;
    end

    if debugMode == true
        fprintf("\n");
    end
end

function [fOpt] = getFOpt(matr, matrSIZ)
    fOpt = 0;
    [numRows, numCols] = size(matr);

    for i = 1 : numRows
        for j = 1 : numCols
            if matrSIZ(i, j) == 1 
                fOpt = fOpt + matr(i, j);
            end
        end
    end
end
