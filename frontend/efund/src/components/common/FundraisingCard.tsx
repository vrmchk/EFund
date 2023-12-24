import { Card, CardMedia, Typography, LinearProgress, Box, CardContent, Link } from '@mui/material';
import Fundraising from '../../models/Fundraising';

const FundraisingCard = (props: { fundraising: Fundraising }) => {
    const { balance, goal, currencyCode, sendUrl } = props.fundraising.monobankJar;
    const progress = (balance / goal) * 100;

    return (
        <Card
            sx={{
                display: 'flex',
                position: 'relative',
                flexDirection: 'row',
                alignItems: 'center',
                gap: '15px',
                padding: '10px',
                paddingLeft: '20px',
                height: '140px',
                width: 600,
            }}
        >
            <CardMedia
                component="img"
                sx={{ width: 50, height: 50 }}
                image={props.fundraising.avatarUrl}
                alt={props.fundraising.title}
            />
            <CardContent sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column', gap: 0.5 }}>
                <Typography variant="h5" component="div">
                    {props.fundraising.title}
                </Typography>
                <Typography variant="body2" color="text.secondary">
                    {props.fundraising.description}
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
                    <Typography variant="body2" color="text.secondary">
                        {balance} {currencyCode}
                    </Typography>
                    <LinearProgress sx={{ flexGrow: 1 }} variant="determinate" value={progress} />
                    <Typography variant="body2" color="text.secondary">
                        {goal} {currencyCode}
                    </Typography>
                    <Link
                        sx={{
                            display: 'inline-block',
                            backgroundColor: 'primary.main',
                            color: 'white',
                            padding: '8px 16px',
                            borderRadius: '4px',
                            textDecoration: 'none',
                            '&:hover': {
                                backgroundColor: 'primary.dark',
                            },
                        }}
                        href={sendUrl}
                        underline="none"
                        target="_blank"
                        rel="noopener noreferrer"
                    >
                        More Info
                    </Link>
                </Box>
            </CardContent>
        </Card>
    );
};

export default FundraisingCard;