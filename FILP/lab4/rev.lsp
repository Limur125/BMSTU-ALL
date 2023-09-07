(defun swap (i1 i2 lst) (rotatef (nth i1 lst) (nth i2 lst)))

(defun rev(lst)
    (let* 
        (
            (len/2 (floor (/ (length lst) 2)))
            (fhi (maplist #'(lambda (l2) (- (length l2) 1)) (nthcdr len/2 lst)))
            (shi (mapcar #'(lambda (l1) (- (length lst) l1 1)) fhi))
        )
        (and (mapcar #'(lambda (fi si) (swap fi si lst)) fhi shi) lst)
    )
)

(defun my-reverse (lst)
    (cond ((null (cdr lst)) lst)
        (t (rplacd (last (my-reverse (cdr lst))) (car lst)))
    )
)

(defun find-list (lst)
    (cond ((null lst) nil)
        ((and (listp (car lst)) (not (null (car lst)))) (car lst))
        (t (find-list (cdr lst)))
    )
)

(defun mult-all (lst n)
    (and (cond ((null lst) nil )
        (t (and (rplaca lst (* (car lst) n)) (mult-all (cdr lst) n)))
    ) lst)
)