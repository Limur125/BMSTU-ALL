(defun f1 (ar1 ar2 ar3 ar4) (list (list ar1 ar2) (list ar3 ar4)))
(f1 'a 'b 'c 'd)
((lambda (ar1 ar2 ar3 ar4) (list (list ar1 ar2) (list ar3 ar4))) 'a 'b 'c 'd)

(defun f2 (ar1 ar2) (list (list ar1) (list ar2)))
(f2 'a 'b)
((lambda (ar1 ar2) (list (list ar1) (list ar2))) 'a 'b)

(defun f3 (ar1) (list (list (list ar1))))
(f3 'a)
((lambda (ar1) (list (list (list ar1)))) 'a)
