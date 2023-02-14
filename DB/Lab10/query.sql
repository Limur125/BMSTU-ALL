select count(vg.title), gr.esrb_rating, vg.release
from VideoGames as vg join GameRating as gr on vg.title = gr.title
group by gr.esrb_rating, vg.release
