import { Box, Grid, Button, Typography, useTheme } from "@mui/material";
import Accordion from "@mui/material/Accordion";
import AccordionSummary from "@mui/material/AccordionSummary";
import AccordionDetails from "@mui/material/AccordionDetails";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { tokens } from "./theme";

const ServiceOrderTable = ({
  order,
  handleDelete,
  handleEdit,
  handleViewAllDispatchers,
}) => {
  const theme = useTheme();
  const colors = tokens(theme.palette.mode);

  return (
    <Box m="20px">
      <Accordion defaultExpanded>
        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
          <Typography color={colors.greenAccent[500]} variant="h5">
            Service Order Details
          </Typography>
        </AccordionSummary>
        <AccordionDetails>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6}>
              <Typography variant="body1">
                <strong>Order ID:</strong> {order.id}
              </Typography>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Typography variant="body1">
                <strong>Status:</strong> {order.status}
              </Typography>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Typography variant="body1">
                <strong>Progress:</strong> {order.progress}
              </Typography>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Typography variant="body1">
                <strong>Created At:</strong> {new Date(order.createdAt).toLocaleString()}
              </Typography>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Typography variant="body1">
                <strong>Dispatchers:</strong> {order.dispatchers.length} 
              </Typography>
             
            </Grid>
            <Grid item xs={12} sm={6} textAlign="right">
              <Button
                variant="contained"
                color="secondary"
                onClick={() => handleEdit(order)}
                sx={{ mr: 1 }}
              >
                Edit
              </Button>
              <Button
                variant="contained"
                color="error"
                onClick={() => handleDelete(order.id)}
              >
                Delete
              </Button>
            </Grid>
          </Grid>
        </AccordionDetails>
      </Accordion>

      <Accordion defaultExpanded>
        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
          <Typography color={colors.greenAccent[500]} variant="h5">
            Company
          </Typography>
        </AccordionSummary>
        <AccordionDetails>
          <Typography variant="body2">
            Company-related details will be displayed here.
          </Typography>
        </AccordionDetails>
      </Accordion>

      <Accordion defaultExpanded>
        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
          <Typography color={colors.greenAccent[500]} variant="h5">
            Dispatcher
          </Typography>
        </AccordionSummary>
        <AccordionDetails>
          <Typography variant="body2">
            Dispatcher-related details will be displayed here.
          </Typography>
        </AccordionDetails>
      </Accordion>

      <Accordion defaultExpanded>
        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
          <Typography color={colors.greenAccent[500]} variant="h5">
            Articles
          </Typography>
        </AccordionSummary>
        <AccordionDetails>
          <Typography variant="body2">
            Articles-related details will be displayed here.
          </Typography>
        </AccordionDetails>
      </Accordion>

      <Accordion defaultExpanded>
        <AccordionSummary expandIcon={<ExpandMoreIcon />}>
          <Typography color={colors.greenAccent[500]} variant="h5">
            Technician
          </Typography>
        </AccordionSummary>
        <AccordionDetails>
          <Typography variant="body2">
            Technician-related details will be displayed here.
          </Typography>
        </AccordionDetails>
      </Accordion>
    </Box>
  );
};

export default ServiceOrderTable;
