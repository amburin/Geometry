select a.name ,t.name
from articles a
left join articles_tags at on a.id = at.articleid
left join tags t on at.tagid = t.id