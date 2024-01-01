import { Card, CardMedia, Typography, LinearProgress, Box, CardContent, Button, Chip, Link } from '@mui/material';
import Fundraising from '../../models/Fundraising';

interface FundraisingCardProps {
    fundraising: Fundraising;
    size: 'small' | 'large';
    selected?: boolean;
    onClick?: (id: string) => void;
}

const FundraisingCard = (props: FundraisingCardProps) => {
    const { balance, goal, currencyCode, sendUrl } = props.fundraising.monobankJar;
    const textColor = props.selected ? 'primary.contrastText' : 'text.primary';
    const progress = (balance / goal) * 100;

    return (
        props.size === 'small' ?
            <>
                <Card
                    sx={{
                        bgcolor: props.selected ? 'primary.light' : 'background.paper',
                        display: 'flex',
                        position: 'relative',
                        flexDirection: 'row',
                        alignItems: 'center',
                        gap: '15px',
                        padding: '10px',
                        paddingLeft: '20px',
                        height: '100%',
                        width: 600,
                    }}
                >
                    <CardMedia
                        component="img"
                        sx={{
                            width: 75,
                            height: 75,
                            objectFit: 'initial',
                        }}
                        image={props.fundraising.avatarUrl}
                        alt={props.fundraising.title}
                    />
                    <CardContent sx={{
                        flexGrow: 1,
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 0.5
                    }}>
                        <Typography variant="h5" textOverflow={'ellipsis'} overflow={'hidden'} maxHeight={'30px'} component="div" color={textColor}>
                            {props.fundraising.title.length > 37 ? props.fundraising.title.substring(0, 37) + '...' : props.fundraising.title}
                        </Typography>
                        <Typography variant="body2" textOverflow={'ellipsis'} overflow={'hidden'} maxHeight={'20px'} color={textColor}>
                            {props.fundraising.description.length > 69 ? props.fundraising.description.substring(0, 69) + '...' : props.fundraising.description}
                        </Typography>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                alignItems: 'center',
                                gap: '10px',
                                marginTop: '10px',
                            }}
                        >
                            <Typography variant="body2" color={textColor}>
                                {balance} {currencyCode}
                            </Typography>
                            <LinearProgress sx={{ flexGrow: 1 }} variant="determinate" value={progress} />
                            <Typography variant="body2" color={textColor}>
                                {goal} {currencyCode}
                            </Typography>
                            <Button
                                color={props.selected ? 'secondary' : 'primary'}
                                variant="contained"
                                onClick={() => props.onClick && props.onClick(props.fundraising.id)}>
                                More Info
                            </Button>
                        </Box>
                    </CardContent>
                </Card >
            </>
            :
            <>
                <Card
                    sx={{
                        display: 'flex',
                        position: 'relative',
                        flexDirection: 'row',
                        alignItems: 'stretch',
                        gap: '15px',
                        padding: '10px',
                        height: '100%',
                        width: '100%',
                    }}
                >
                    <CardMedia
                        component="img"
                        sx={{
                            width: 250,
                            height: 250,
                            objectFit: 'initial',
                        }}
                        image={props.fundraising.avatarUrl}
                        alt={props.fundraising.title}
                    />
                    <CardContent sx={{
                        flexGrow: 1,
                        display: 'flex',
                        flexDirection: 'column',
                        gap: 0.5
                    }}>
                        <Typography variant="h4" component="div" textOverflow={'ellipsis'} overflow={'hidden'} maxHeight={'100px'} color={textColor}>
                            Lorem ipsum dolor sit amet consectetur adipisicing elitdddddsss
                        </Typography>
                        <Box sx={{ display: 'flex', gap: 1 }}>
                            {props.fundraising.tags.map((tag, index) => (
                                <Chip
                                    key={index}
                                    label={tag}
                                    color='primary'
                                    variant='filled'
                                />
                            ))}
                        </Box>
                        <Typography variant="body1" textOverflow={'ellipsis'} overflow={'hidden'} color={textColor}>
                            Lorem ipsum dolor sit amet consectetur adipisicing elit. Cumque hic vel quo Lorem ipsum dolor sit amet consectetur adipisicing elit. Ipsum, doloribus. Sint voluptatibus autem molestias explicabo facere nulla repudiand Lorem ipsum, dolor sit amet consectetur adipisicing elit. Magni, totam! Iusto laudantium libero officia maxime minima, inventore cum, quaerat delectus quam, deleniti mollitia rerum sint ullam tempore! Saepe, voluptatem rem! ae eos, repellat doloribus qui, provident dolor esse voluptas totam reprehenderit dicta ullam. s? Reprehenderit minima natus in corrupti atque saepe ut, ipsam, aliquam laborum quam sit esse id nostrum voluptatum vel.
                        </Typography>
                        <Box
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                alignItems: 'center',
                                alignSelf: 'center',
                                gap: '10px',
                                width: '70%',
                                marginTop: '10px',
                            }}
                        >
                            <Typography variant="body2">
                                {balance} {currencyCode}
                            </Typography>
                            <LinearProgress color='primary' sx={{ flexGrow: 1 }} variant="determinate" value={progress} />
                            <Typography variant="body2" color={textColor}>
                                {goal} {currencyCode}
                            </Typography>
                        </Box>
                        <Link
                            href={sendUrl}
                            sx={{
                                alignSelf: 'center',
                                marginTop: '10px',
                                textDecoration: 'none',
                            }}
                            target='_blank'
                            rel="noopener noreferrer"
                        >
                            <Button variant="contained" color="primary" style={{ fontWeight: 'bold' }}>
                                Go to Monobank
                            </Button>
                        </Link>
                    </CardContent>
                </Card>
            </>
    );
};

export default FundraisingCard;